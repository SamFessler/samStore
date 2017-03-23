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

        private static List<ProductModel> Trees = new List<ProductModel>();


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
            //using (SamStoreEntities entities = new SamStoreEntities())
            //{
            //    AspNetUser currentUser = entities.AspNetUsers.Single(x => x.UserName == User.Identity.Name);
            //    Order o = currentUser.Orders.FirstOrDefault(x => x.Completed == null);

            // aspnetuser needs to be linked to orders with foreignkey
            //}



                List<ProductModel> cart = this.Session["Cart"] as List<ProductModel>;
            if(cart == null)
            {
                cart = new List<ProductModel>();
            }

            cart.Add(model);
            this.Session.Add("Cart", cart);

            //repalced by using session to pass cart model
            //this.Response.SetCookie(new HttpCookie("ProductName", "-1"));
            //this.Response.SetCookie(new HttpCookie("ProductId", model.Id.ToString()));
            //this.Response.SetCookie(new HttpCookie("ProductPrice", model.TreePrice.ToString()));

            TempData.Add("AddedToCart", true);

            return RedirectToAction("Index", "Cart");
        }
    }
}