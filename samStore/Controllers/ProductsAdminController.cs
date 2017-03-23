using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using samStore.Models;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace samStore.Controllers
{

    [Authorize]
    public class ProductsAdminController : Controller
    {
        private SamStoreEntities db = new SamStoreEntities();

        // GET: ProductsAdmin
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: ProductsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,ProductName,ProductDescription,ProductPrice,Active,Inventory,TreeSpecies,TreeSkill,CreatedDate,ModifiedDate")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.UtcNow;
                db.Entry(product).State = EntityState.Modified;

                string filename = image.FileName;
                int i = 1;
                while(System.IO.File.Exists(Server.MapPath("/Content/Images/"+ filename)))
                {
                    filename = System.IO.Path.GetFileNameWithoutExtension(filename) + i.ToString() + System.IO.Path.GetExtension(filename);
                    i++;
                }

                image.SaveAs(Server.MapPath("/Content/Images/" + filename));
                if (db.ProductImages.Any(x => x.ID == product.ID))
                {
                    ProductImage newImage = db.ProductImages.FirstOrDefault(x => x.ProductID == product.ID);
                    newImage.ImagePath = "/Content/Images/" + filename;
                    newImage.ModifiedDate = DateTime.UtcNow;

                }
                else
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ImagePath = "/Content/Images/" + image.FileName,
                        CompletedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow

                    });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: ProductsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,ProductName,ProductDescription,ProductPrice,Active,Inventory,TreeSpecies,TreeSkill,CreatedDate,ModifiedDate")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {

                product.ModifiedDate = DateTime.UtcNow;
                db.Entry(product).State = EntityState.Modified;

                string filename = image.FileName;
                if (ConfigurationManager.AppSettings["UseLocalStorage"] == "true")
                {

                   
                    int i = 1;
                    while (System.IO.File.Exists(Server.MapPath("/Content/Images/" + filename)))
                    {
                        filename = System.IO.Path.GetFileNameWithoutExtension(filename) + i.ToString() + System.IO.Path.GetExtension(filename);
                        i++;
                    }

                }
                else
                {
                    //use blob
                    CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
                    var blobClient = account.CreateCloudBlobClient();

                    var rootContainer = blobClient.GetRootContainerReference();

                    rootContainer.CreateIfNotExists();
                    rootContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                    var blob = rootContainer.GetBlobReference("/Content/Images/" + filename);
                    blob.UploadFromStream(image.InputStream);

                    filename = blob.Uri.ToString();
                }


                image.SaveAs(Server.MapPath("/Content/Images/" + image.FileName));
                if(db.ProductImages.Any(x => x.ID == product.ID))
                {
                    ProductImage newImage = db.ProductImages.FirstOrDefault(x => x.ProductID == product.ID);
                    newImage.ImagePath = "/Content/Images/" + image.FileName;
                    newImage.ModifiedDate = DateTime.UtcNow;

                }
                else
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ImagePath = "/Content/Images/" + image.FileName,
                        CompletedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow

                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: ProductsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
