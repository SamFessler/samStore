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

        [HttpPost]
        public ActionResult ValidateAddress(string street1, string street2, string city, string state, string zip)
        {
            string authId = ConfigurationManager.AppSettings["SmartyStreets.AuthID"];
            string authToken = ConfigurationManager.AppSettings["SmartyStreets.AuthToken"];
            SmartyStreets.USStreetApi.ClientBuilder builder = new SmartyStreets.USStreetApi.ClientBuilder(authId, authToken);
            SmartyStreets.USStreetApi.Client client = builder.Build();

            SmartyStreets.USStreetApi.Lookup lookup = new SmartyStreets.USStreetApi.Lookup();

            lookup.City = city;
            lookup.State = state;
            lookup.Street = street1;
            lookup.Street2 = street2;
            lookup.ZipCode = zip;

            client.Send(lookup);

            var results = lookup.Result;

            return Json(results);
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