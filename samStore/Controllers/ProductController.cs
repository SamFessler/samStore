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
        public ActionResult Index(int? id)
        {
            if (Trees.Count == 0)
            {

                Trees.Add(new ProductModel {
                    Id = 1,
                    TreeName = "Japanese Black Pine",
                    TreeSpecies = "Pinus thunbergii",
                    TreeType = "coniferous",
                    TreeImage = new string[]{ "/Content/Images/japaneseBlackPine.jpg" },
                    TreePrice = 25.00M,
                    TreeDescription ="This is an amazing tree" });

                Trees.Add(new ProductModel {
                    Id = 2,
                    TreeName = "Monterey cypress",
                    TreeSpecies = "Cupressus macrocarpa",
                    TreeType = "cypress",
                    TreeImage = new string[] { "/Content/Images/montreyCypress.jpg" },
                    TreePrice = 40.5M,
                    TreeDescription = "The Monterey cypress is a species of cypress native to the Central Coast of California." });

            }

            if(id == 1)
            {
                return View(Trees[0]);
            }
            if (id == 2)
            {
                return View(Trees[1]);
            }
           else
            {
                return HttpNotFound();
            }
        }

        //submit button for add to cart, Post Product
        [HttpPost]
        public ActionResult Index(ProductModel model)
        {

            this.Response.SetCookie(new HttpCookie("ProductName", "-1"));
            this.Response.SetCookie(new HttpCookie("ProductId", model.Id.ToString()));
            this.Response.SetCookie(new HttpCookie("ProductPrice", model.TreePrice.ToString()));

            return RedirectToAction("Index", "Cart");
        }
    }
}