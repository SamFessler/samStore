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

                model.Items = o.OrderProducts.Select(x => new CartItemModel
                {
                    Product = new ProductModel
                    {
                        Id = x.Product.ProductID,
                        TreeDescription = x.Product.ProductDescription,
                        TreeName = x.Product.ProductName,
                        TreePrice = x.Product.ProductPrice,
                        TreeImage = x.Product.ProductImages.Select(y => y.ImagePath)
                    },
                    Quantity = x.Quantity

                }).ToArray();

                model.SubTotal = o.OrderProducts.Sum(x => x.Product.ProductPrice * x.Quantity);
            }
            ViewBag.PageGenerationTime = DateTime.Now;
            return View(model);
        }
    }
}