using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using samStore.Models;

namespace samStore.Controllers
{
    [CartItemCalculator]
    public class HomeController : Controller
    {
        OrderContainerModel ModelList = new OrderContainerModel();

        SamStoreEntities entities = new SamStoreEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ActionName("Profile")]
        public ActionResult ProfileAction()
        {
                if (User.Identity.IsAuthenticated)
                {
                    AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
                    
                    
                    ModelList.Orders = currentUser.Orders.Select(x =>  new OrderUserModel  
                    {
                        Order = new OrderModel
                        {
                            Id = x.ID,
                            //ShippingAddress = x.Address.ShippingAddress1,
                            EmailUsed = x.PurchaserEmail,
                           

                            Products = x.OrderProducts.Select(y => new OrderProduct {
                                Product = new Product
                                {
                                    ProductName = y.Product.ProductName,
                                    ProductPrice = y.Product.ProductPrice
                                },
                                 Quantity = y.Quantity,
                               

                            })
                          
                            
                        },

                    }).ToArray();

                    
                }


            


            ViewBag.Message = "Your Profile";
            return View(ModelList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entities.Dispose();
            }
            base.Dispose(disposing);
        }

    }

   
}