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

        //public ActionResult Profile()
        //{
        //    OrderContainerModel ModelList = new OrderContainerModel();

        //    using (SamStoreEntities entities = new SamStoreEntities())
        //    {

                

        //        int i = 0;

        //        if (User.Identity.IsAuthenticated)
        //        {
        //            AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);

        //            Order o = currentUser.Orders.FirstOrDefault(x => new OrderUserModel
        //            {
        //                Order = new OrderModel
        //                {
        //                    Id = x.ID,
        //                    ShippingAddress = x.Address.ShippingAddress1,

        //               },

        //            }).ToArray();
        //                   // userOrders.Order[i] = currentUser.Orders.FirstOrDefault(x => x.Completed != null && x.ID != userOrders.OrdersNum[i])
        //                   //userOrder.OrdersNum[i]

        //        }

        //    }

        //    ViewBag.Message = "Your Profile";

        //    return View();
        //}
    }

   
}