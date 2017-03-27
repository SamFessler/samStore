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
            if(ModelState.IsValid)
            {
                //validated
                //TODO persist this order to the database and redirect to reciept page
                //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SamStore"].ConnectionString;

                string sendGridKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];

                SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridKey);
                SendGrid.Helpers.Mail.SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();
                message.Subject = "recipt for order #0000000";
               

                var from = new EmailAddress("test@ArtOfBonsai.com", "Art Of Bonsai Support");

                var subject = "Order Confermation from Art Of Bonsai";
                var to = new EmailAddress("test@example.com", "Example User");
                var plainTextContent = "Thank you for your order";
                var htmlContent = "<strong>Tracking number is: </strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                
                
                await client.SendEmailAsync(message);




                using (SamStoreEntities entities = new SamStoreEntities())
                {
                    string uniqueName = Guid.NewGuid().ToString();

                    Order newOrder = new Models.Order(); //create object 
                    newOrder.PurchaserEmail = uniqueName + "@codingtmeple.com";
                    newOrder.ShipCareOf = uniqueName;
                    newOrder.CreatedDate = DateTime.UtcNow;
                    newOrder.CreatedDate = DateTime.UtcNow;
                    entities.Orders.Add(newOrder);//attach to collection
                    entities.SaveChanges();//save changes


                    int ID = newOrder.ID;
                    // entities.sp_CompleteOrder(ID);  can call stored proccedure and pass ID to complete part of code


                    string merchandId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
                    string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
                    string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
                    string enviornment = ConfigurationManager.AppSettings["Braintree.Environment"];

                    Braintree.BraintreeGateway braintree = new Braintree.BraintreeGateway(enviornment, merchandId, publicKey, privateKey);

                    Braintree.TransactionRequest newTransaction = new Braintree.TransactionRequest();
                    newTransaction.Amount = 1m; //hardcode the price

                    Braintree.TransactionCreditCardRequest creditCard = new Braintree.TransactionCreditCardRequest();
                    creditCard.CardholderName = model.CreditCardName;
                    creditCard.CVV = model.CardVerificationValue;
                    creditCard.ExpirationMonth = model.CreditCardExperation.Value.Month.ToString().PadLeft(2,'0');
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
                       
                     }

                }
            }
            else
            {
                //invalid
            }
            return View(model);
        }

    }
}