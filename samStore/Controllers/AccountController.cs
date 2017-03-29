using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using samStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using System.Configuration;

namespace samStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {

            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                using(IdentityModels entities = new IdentityModels())
                {
                    var userStore = new UserStore<User>(entities);
                    
                    var manager = new UserManager<User>(userStore);

                    var user = new User()
                    {
                        UserName = model.EmailAddress,
                        Email = model.EmailAddress,
                    
                    };

                    IdentityResult result = manager.Create(user, model.Password);
                    


                    if (result.Succeeded)
                    {
                        //added 
                        User u = manager.FindByName(model.EmailAddress);
                        string confermationToken = manager.GenerateEmailConfirmationToken(u.Id);

                        string sendGridKey

                        //put in email for confermation of account
                        //pass confermation token in the email and include a link to confirm account

                        
                        return RedirectToAction("ConfirmSent");

                    }
                    else
                    {
                        ModelState.AddModelError("EmailAddress", "Unable to register with this email address");
                    }


                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        public ActionResult ConfirmSent()
        {
            return View();
        }

        public ActionResult Confirm(string id, string email)
        {
            var userStore -new UserStore<User>(entities);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (IdentityModels entities = new IdentityModels())
                {
                    var userStore = new UserStore<User>(entities);

                    var manager = new UserManager<User>(userStore);

                    var user = manager.FindByEmail(model.EmailAddress);

                    if (manager.CheckPassword(user, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.EmailAddress, true);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("Email Address","Could not sign in with this username/password");
                    }
                }

                }
            return View(model);
        }
    }
}