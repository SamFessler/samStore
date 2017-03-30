using System;
using System.Collections.Generic;
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
                    //product.ModifiedDate = DateTime.UtcNow;
                    //product.CreatedDate = DateTime.UtcNow;
                    return View(model);
                }
                else
                {
                    return HttpNotFound(string.Format("ID {0} Not Found", id));
                }


            }
        }

        //        if (Trees.Count == 0)
        //        {

        //            Trees.Add(new ProductModel {
        //                Id = 1,
        //                TreeName = "Japanese Black Pine",
        //                TreeSpecies = "Pinus thunbergii",
        //                TreeType = "coniferous",
        //                TreeImage = new string[] { "/Content/Images/japaneseBlackPine.jpg" },
        //                TreePrice = 25.00M,
        //                TreeDescription = "This is an amazing tree" });

        //            Trees.Add(new ProductModel {
        //                Id = 2,
        //                TreeName = "Monterey cypress",
        //                TreeSpecies = "Cupressus macrocarpa",
        //                TreeType = "cypress",
        //                TreeImage = new string[] { "" },
        //                TreePrice = 40.5M,
        //                TreeDescription = "The Monterey cypress is a species of cypress native to the Central Coast of California." });

        //        }

        //    if(id == 1)
        //    {
        //        return View(Trees[0]);
        //    }
        //    if (id == 2)
        //    {
        //        return View(Trees[1]);
        //    }
        //   else
        //    {
        //        return HttpNotFound();
        //    }
        //}

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

                        o.CreatedDate = DateTime.UtcNow;
                        o.ModifiedDate = DateTime.UtcNow;
                    }
                    var product = o.OrderProducts.FirstOrDefault(x => x.ProductID == model.Id);
                    if (product == null)
                    {
                        product = new OrderProduct();
                        product.ProductID = model.Id ?? 0;
                        product.Quantity = 0;
                        product.CreatedDate = DateTime.UtcNow;
                        product.ModifiedDate = DateTime.UtcNow;
                        o.OrderProducts.Add(product);
                        o.CreatedDate = DateTime.UtcNow;
                        o.ModifiedDate = DateTime.UtcNow;
                    }
                    product.Quantity += 1;
                }
                else
                {
                    Order o = null;
                    o.CreatedDate = DateTime.UtcNow;
                    o.ModifiedDate = DateTime.UtcNow;
                    if (Request.Cookies.AllKeys.Contains("orderNumber"))
                    {
                        Guid orderNumber = Guid.Parse(Request.Cookies["orderNumber"].Value);
                        o = entities.Orders.FirstOrDefault(x => x.Completed == null && x.OrderNumber == orderNumber);
                        o.CreatedDate = DateTime.UtcNow;
                        o.ModifiedDate = DateTime.UtcNow;
                    }
                    if (o == null)
                    {
                        o = new Order();
                        o.OrderNumber = Guid.NewGuid();
                        entities.Orders.Add(o);
                        Response.Cookies.Add(new HttpCookie("orderNumber", o.OrderNumber.ToString()));

                        o.CreatedDate = DateTime.UtcNow;
                        o.ModifiedDate = DateTime.UtcNow;
                    }
                    var product = o.OrderProducts.FirstOrDefault(x => x.ProductID == model.Id);
                    if (product == null)
                    {
                        product = new OrderProduct();
                        product.ProductID = model.Id ?? 0;
                        product.Quantity = 0;
                        product.CreatedDate = DateTime.UtcNow;
                        product.ModifiedDate = DateTime.UtcNow;

                        o.OrderProducts.Add(product);
                        o.CreatedDate = DateTime.UtcNow;
                        o.ModifiedDate = DateTime.UtcNow;
                    }
                product.Quantity += 1;
                }
                entities.SaveChanges();
                TempData.Add("AddedToCart", true);
            }
            return RedirectToAction("Index", "Cart");

        }
            //List<ProductModel> cart = this.Session["Cart"] as List<ProductModel>;
            //if(cart == null)
            //{
            //    cart = new List<ProductModel>();
            //}

            //cart.Add(model);
            //this.Session.Add("Cart", cart);

            ////repalced by using session to pass cart model
            ////this.Response.SetCookie(new HttpCookie("ProductName", "-1"));
            ////this.Response.SetCookie(new HttpCookie("ProductId", model.Id.ToString()));
            ////this.Response.SetCookie(new HttpCookie("ProductPrice", model.TreePrice.ToString()));

            //TempData.Add("AddedToCart", true);

            //return RedirectToAction("Index", "Cart");
    }
}