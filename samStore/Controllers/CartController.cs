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
        private SamStoreEntities db = new SamStoreEntities();

        // GET: Cart
        public ActionResult Index()
        {
            CartModel model = new CartModel();

            using (SamStoreEntities entities = new SamStoreEntities())
            {

                Order o = null;
                //OrderProduct item = null;
                if (User.Identity.IsAuthenticated)
                {
                    AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
                    o = currentUser.Orders.FirstOrDefault(x => x.Completed == null);
                    //item = o.OrderProducts.FirstOrDefault(x => x.OrderID == o.ID);
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
                        //item = o.OrderProducts.FirstOrDefault(x => x.OrderID == o.ID);
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
                    Quantity = x.Quantity,
                    ///TODO: fix item to OrdersProducts Subtotal
                }).ToArray();

                

                model.SubTotal = o.OrderProducts.Sum(x => x.Product.ProductPrice * x.Quantity);
            }
            //ViewBag.PageGenerationTime = DateTime.Now;
            return View(model);
        }

        public ActionResult RemoveItem(int id)
        {
            using (SamStoreEntities entities = new SamStoreEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
                    Order o = currentUser.Orders.FirstOrDefault(x => x.Completed == null);
                    OrderProduct item = o.OrderProducts.FirstOrDefault(x => x.ProductID == id);
                    if (item.Quantity == 1)
                    {
                        item.Product.OrderProducts = null;
                        if (o.OrderProducts.Count == 1)
                        {
                            o.OrderProducts = null;
                        }
                    }
                    else
                    {
                        item.Quantity -= 1;
                    }
                }
                else
                {
                    if (Request.Cookies.AllKeys.Contains("orderNumber"))
                    {
                        Guid orderNumber = Guid.Parse(Request.Cookies["orderNumber"].Value);
                        Order o = entities.Orders.FirstOrDefault(x => x.Completed == null && x.OrderNumber == orderNumber);
                        OrderProduct item = o.OrderProducts.FirstOrDefault(x => x.ProductID == id);
                        if (item.Quantity == 1)
                        {
                            item.Product.OrderProducts = null;
                            if (o.OrderProducts.Count == 1)
                            {
                                o.OrderProducts = null;
                            }
                        }
                        else
                        {
                            item.Quantity -= 1;
                        }
                    }

                }
                entities.SaveChanges();
            }
            return RedirectToAction("Index", "Cart");
        }
    }
}