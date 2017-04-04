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
using System.Security.Cryptography;
using System.Text;
using Microsoft.Ajax.Utilities;



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
                    manager.UserTokenProvider = new EmailTokenProvider<User>();

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


                        //Create a customer record in Braintree:
                        string merchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
                        string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
                        string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
                        string environment = ConfigurationManager.AppSettings["Braintree.Environment"];
                        Braintree.BraintreeGateway braintree = new Braintree.BraintreeGateway(environment, merchantId, publicKey, privateKey);
                        Braintree.CustomerRequest customer = new Braintree.CustomerRequest();
                        customer.CustomerId = u.Id;
                        customer.Email = u.Email;

                        var r = await braintree.Customer.CreateAsync(customer);
                        


                        string confermationToken = manager.GenerateEmailConfirmationToken(u.Id);

                        string sendGridKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];

                        SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridKey);
                        SendGrid.Helpers.Mail.SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();
                        message.Subject = string.Format("Please confirm your account");
                        message.From = new SendGrid.Helpers.Mail.EmailAddress("Admin@apples4pears.net", "Art Of Bonsai Admin");
                        message.AddTo(new SendGrid.Helpers.Mail.EmailAddress(model.EmailAddress));
                        SendGrid.Helpers.Mail.Content contents = new SendGrid.Helpers.Mail.Content("text/html", string.Format("<a href=\"{0}\">Confirm Account</a>", Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Confirm/" + confermationToken + "?email=" + model.EmailAddress));

                        message.AddContent(contents.Type, contents.Value);
                        SendGrid.Response response = await client.SendEmailAsync(message);



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

         
            if (Request.Cookies["orderNumber"] != null)
            {
                var c = new HttpCookie("orderNumber");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

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
            using (IdentityModels entities = new IdentityModels())
            {
                var userStore = new UserStore<User>(entities);

                var manager = new UserManager<User>(userStore);
                manager.UserTokenProvider = new EmailTokenProvider<User>();
                var user = manager.FindByName(email);
                if (user != null)
                {
                    var result = manager.ConfirmEmail(user.Id, id);
                    if (result.Succeeded)
                    {
                        TempData.Add("AccountConfirmed", true);
                        return RedirectToAction("Login");
                    }
                }
            }

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Ok()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        public ActionResult ResetPasswordPage(string code, string email)
        {
            return View(new ResetPasswordViewModel(code,email));
        }

        public ActionResult ResetSent()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (IdentityModels entities = new IdentityModels())
                {

                    var userStore = new UserStore<User>(entities);
                    var manager = new UserManager<User>(userStore);

                    manager.UserTokenProvider = new EmailTokenProvider<User>();

                    var user = manager.FindByName(model.Email);
                    // If user has to activate his email to confirm his account, the use code listing below
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    //{
                    //    return Ok();
                    //}
                    

                    string code = await manager.GeneratePasswordResetTokenAsync(user.Id);


                    string sendGridKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];
                    SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridKey);
                    SendGrid.Helpers.Mail.SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();
                    message.Subject = string.Format("Reset Password");
                    message.From = new SendGrid.Helpers.Mail.EmailAddress("Admin@apples4pears.net", "Art Of Bonsai Admin");
                    message.AddTo(new SendGrid.Helpers.Mail.EmailAddress(model.Email));

                    SendGrid.Helpers.Mail.Content contents = new SendGrid.Helpers.Mail.Content("text/html", string.Format("<a href=\"{0}\">Reset Your Password</a>", Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/ResetPasswordPage/" + code + "?EmailAddress=" + model.Email ));

                    message.AddContent(contents.Type, contents.Value);
                    SendGrid.Response response =  await client.SendEmailAsync(message);

                    //await client.SendEmailAsync(user.Id, "Reset Password", $"Please reset your password by using this {code}");
                    return RedirectToAction("ResetSent");
                }
                
            }
            return View();

            // If we got this far, something failed, redisplay form

        }

        public ActionResult ResetPassword(string code, string email, string NewPassword)
        {
            using (IdentityModels entities = new IdentityModels())
            {
                var userStore = new UserStore<User>(entities);

                var manager = new UserManager<User>(userStore);
                var user = manager.FindByName(email);

                manager.UserTokenProvider = new EmailTokenProvider<User>();

                

                if (user != null)
                {
                    var result = manager.ResetPassword(user.Id, code, NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }

                }
            }

            return RedirectToAction("Index", "Home");

        }


        //public ActionResult Reset(string id, string email)
        //{
        //    using (IdentityModels entities = new IdentityModels())
        //    {
        //        var userStore = new UserStore<User>(entities);

        //        var manager = new UserManager<User>(userStore);
        //        manager.UserTokenProvider = new EmailTokenProvider<User>();
        //        var user = manager.FindByName(email);
        //        if (user != null)
        //        {
        //            var result = manager.ResetPassword(user.Id, id);
        //            if (result.Succeeded)
        //            {
        //                TempData.Add("PasswordReset", true);
        //                return RedirectToAction("Login");
        //            }
        //        }
        //    }
        //}

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