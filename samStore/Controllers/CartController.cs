using samStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace samStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            CartModel model = new CartModel();
            List<ProductModel> cart = Session["Cart"] as List<ProductModel>;
            if(cart == null)
            {
                cart = new List<ProductModel>();
            }

            ViewBag.ItemsInCart = 0;
            model.Items = new CartItemModel[cart.Count];
            model.SubTotal = 0;
            for (int i = 0; i < cart.Count; i++)
            {
                model.Items[i] = new CartItemModel
                {
                    Product = cart[i],
                    Quantity = 1

                };

                model.SubTotal += (model.Items[i].Product.TreePrice) * (model.Items[i].Quantity);
                ViewBag.ItemsInCart++;//TODO needs to be run many places
            }


           
            return View(model);
        }
    }
}