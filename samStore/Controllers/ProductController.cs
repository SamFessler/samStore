using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using samStore.Models;

namespace samStore.Controllers
{
    public class ProductController : Controller
    {

        //private static List<ProductModel> Trees = new List<ProductModel>();


        // GET: Product
        [OutputCache(Duration = 300)]
        public ActionResult Index(int? id)
        {
            using (SamStoreEntities entities = new SamStoreEntities())
            {
                var product = entities.Products.Find(id);
                if (product != null)
                {
                    ProductModel model = new ProductModel();
                    model.Id = product.ID;
                    model.TreeDescription = product.ProductDescription;
                    model.TreePrice = product.ProductPrice;
                    model.TreeName = product.ProductName;
                    model.TreeImage = product.ProductImages.Select(x => x.ImagePath).ToArray();
                    return View(model);
                }
                else
                {
                    return HttpNotFound(string.Format("ID {0} Not Found", id));
                }


            }
        }



        //submit button for add to cart, Post Product
        [HttpPost]
        public ActionResult Index(ProductModel model)
        {
            using (SamStoreEntities entities = new SamStoreEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
                    Order o = currentUser.Orders.FirstOrDefault(x => x.Completed == null);

                    if (o == null)
                    {
                        o = new Order();
                        o.OrderNumber = Guid.NewGuid();
                        currentUser.Orders.Add(o);

                        o.CreatedDate = DateTime.Now;
                        o.ModifiedDate = DateTime.Now;

                    }
                    var product = o.OrderProducts.FirstOrDefault(x => x.ProductID == model.Id);
                    if (product == null)
                    {
                        product = new OrderProduct();
                        product.ProductID = model.Id ?? 0;
                        product.Quantity = 0;
                        product.CreatedDate = DateTime.Now;
                        product.ModifiedDate = DateTime.Now;
                        o.OrderProducts.Add(product);
                    }
                    product.Quantity += 1;
                }
                else
                {
                    Order o = null;

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

                        o.CreatedDate = DateTime.Now;
                        o.ModifiedDate = DateTime.Now;
                    }
                    var product = o.OrderProducts.FirstOrDefault(x => x.ProductID == model.Id);
                    if (product == null)
                    {
                        product = new OrderProduct();
                        product.ProductID = model.Id ?? 0;
                        product.Quantity = 0;
                        product.CreatedDate = DateTime.Now;
                        product.ModifiedDate = DateTime.Now;

                        o.OrderProducts.Add(product);

                    }
                    product.Quantity += 1;
                }
                entities.SaveChanges();
                ViewBag.ItemsInCart += 1;
                TempData.Add("AddedToCart", true);
            }

            return RedirectToAction("Index", "Cart");

        }

    }
}