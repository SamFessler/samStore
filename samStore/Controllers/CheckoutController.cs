using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using samStore.Models;

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
        public ActionResult Index(CheckoutModel model)
        {
            if(ModelState.IsValid)
            {
                //validated
                //TODO persist this order to the database and redirect to reciept page
                //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SamStore"].ConnectionString;
                
            using(SamStoreEntities entities = new SamStoreEntities())
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