﻿using Microsoft.AspNet.Identity.EntityFramework;
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
                        message.From = new SendGrid.Helpers.Mail.EmailAddress("admin@boardgames.codingtemple.com", "Coding Temple Board Games Administrator");
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