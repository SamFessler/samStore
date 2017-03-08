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
            if(Request.Cookies["ProductId"] != null)
            {
                model.Items = new CartItemModel[]
                {
                    new CartItemModel
                    {
                        Product = new ProductModel
                        {
                            Id = int.Parse(Request.Cookies["ProductID"].Value),
                            TreeName = Request.Cookies["ProductName"].Value,
                            TreePrice = decimal.Parse(Request.Cookies["ProductPrice"].Value),
                        },
                        Quantity = 1
                    }
                };

                model.SubTotal = model.Items[0].Quantity * model.Items[0].Product.TreePrice;
            }
            else
            {
                model.Items = new CartItemModel[0];
                model.SubTotal = 0;
            }
            return View(model);
        }
    }
}