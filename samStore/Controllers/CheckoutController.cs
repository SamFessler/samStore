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
                //todo persist this order to the database and redirect to reciept page
            }
            else
            {
                //invalid
            }
            return View(model);
        }

    }
}