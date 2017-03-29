using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using samStore.Models;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using System.Configuration;

namespace samStore.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            CheckoutModel model = new CheckoutModel();

            return View(model);
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                //validated
                //TODO persist this order to the database and redirect to reciept page
                //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SamStore"].ConnectionString;


                using (SamStoreEntities entities = new SamStoreEntities())
                {
                    Order o = null;
                    if (User.Identity.IsAuthenticated)
                    {
                        AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
                        o = currentUser.Orders.FirstOrDefault(x => x.Completed == null);
                        if (o == null)
                        {
                            o = new Order();
                            o.OrderNumber = Guid.NewGuid();
                            currentUser.Orders.Add(o);
                            entities.SaveChanges();
                        }
                    }
                    else
                    {
                        if (Request.Cookies.AllKeys.Contains("orderNumber"))
                        {
                            Guid orderNumber = Guid.Parse(Request.Cookies["orderNumber"].Value);
                            o = entities.Orders.FirstOrDefault(x => x.Completed == null && x.OrderNumber == orderNumber);
                        }
                        if (o == null)
                        {
                            o = new Order();
                            o.OrderNumber = Guid.NewGuid();
                            entities.Orders.Add(o);
                            Response.Cookies.Add(new HttpCookie("orderNumber", o.OrderNumber.ToString()));
                            entities.SaveChanges();
                        }
                    }
                    if (o.OrderProducts.Sum(x => x.Quantity) == 0)
                    {
                        return RedirectToAction("Index", "Cart");
                    }

                    o.PurchaserEmail = User.Identity.Name;
                    Address newShippingAddress = new Address();

                    newShippingAddress.ShippingAddress1 = model.ShippingAddress1;
                    newShippingAddress.ShippingAddress2 = model.ShippingAddress2;
                    newShippingAddress.ShippingCity = model.ShippingCity;
                    newShippingAddress.ShippingState = model.ShippingState;
                    newShippingAddress.ShippingZip = model.ShippingZip;

                    o.Address1 = newShippingAddress;


                    entities.sp_CompleteOrder(o.ID);


                    string merchandId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
                    string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
                    string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
                    string enviornment = ConfigurationManager.AppSettings["Braintree.Environment"];

                    Braintree.BraintreeGateway braintree = new Braintree.BraintreeGateway(enviornment, merchandId, publicKey, privateKey);

                    Braintree.TransactionRequest newTransaction = new Braintree.TransactionRequest();
                    //newTransaction.Amount = 1m; //hardcode the price
                    newTransaction.Amount = o.OrderProducts.Sum(x => x.Quantity * x.Product.ProductPrice) ?? 0.01m;

                    Braintree.TransactionCreditCardRequest creditCard = new Braintree.TransactionCreditCardRequest();
                    creditCard.CardholderName = model.CreditCardName;
                    creditCard.CVV = model.CardVerificationValue;
                    creditCard.ExpirationMonth = model.CreditCardExperation.Value.Month.ToString().PadLeft(2, '0');
                    creditCard.ExpirationYear = model.CreditCardExperation.Value.Year.ToString();
                    creditCard.Number = model.CreditCardNumber;



                    newTransaction.CreditCard = creditCard;

                    if (User.Identity.IsAuthenticated)
                    {
                        Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
                        search.Email.Is(User.Identity.Name);

                    }

                    Braintree.Result<Braintree.Transaction> result = await braintree.Transaction.SaleAsync(newTransaction);

                    if (!result.IsSuccess())
                    {

                        ModelState.AddModelError("CreditCard", "Unsuccessfull transaction request on credicard payment")


                     };
                }
            }
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

    }
}