using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_Dev3_Nadon_Marc_Andre.Models;
using System.IO;

namespace _420_476_Dev3_Nadon_Marc_Andre.Controllers
{
    public class ProductsController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Products
        public ActionResult Index()
        {
            if(Session["Name"] != null)
            {
                var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);
                return View(products.ToList());
            }
            return RedirectToAction("Login","Users");
            
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Name"] != null)
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
            return RedirectToAction("Login","Users");
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["Name"] != null)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
                return View();
            }
            return RedirectToAction("Login","Users");
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            if (Session["Name"] != null)
            {
                product.Photo = Upload(Request);//try without that part
                if(!product.Photo.Equals("wrong"))
                {
                    if (ModelState.IsValid)
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                        Session["Error2"] = null;
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return RedirectToAction("Create");
            }
            return RedirectToAction("Login","Users");
        }

        [HttpPost]
        private string Upload(HttpRequestBase Request)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    if(fileExtension.Equals(".jpg") || fileExtension.Equals(".png"))
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        file.SaveAs(path);
                        return fileName;
                    }
                    Session["Error2"] = "Mauvais type de fichier";
                    return "wrong";
                }
            }
            Session["Error2"] = "Aucun fichier selectionner";
            return "wrong";
            
        }
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Name"] != null)
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
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return View(product);
            }
            return RedirectToAction("Login","Users");
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            if (Session["Name"] != null)
            {
                if(product.Photo != null)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["Error2"] = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    product.Photo = Upload(Request);//try without that part
                    if (!product.Photo.Equals("wrong"))
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(product).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["Error2"] = null;
                            return RedirectToAction("Index");
                        }
                    }
                    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                    return RedirectToAction("Edit");
                }
                
            }
            return RedirectToAction("Login","Users");
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Name"] != null)
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
            return RedirectToAction("Login","Users");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Name"] != null)
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login","Users");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);

        }
        public void test()
        {
            string test = "chat";
            string res = BCrypt.Net.BCrypt.HashPassword(test);
            bool val = BCrypt.Net.BCrypt.Verify(test, res);
        }

        public ActionResult categoriesAutoComplete()
        {
            string term = Request.QueryString["term"].ToLower();
            var result = from c in db.Categories
                         where c.CategoryName.ToLower().Contains(term)
                         select c.CategoryName;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        
        public ActionResult Research(string mpValue)
        {
            using(NorthwindEntities context = new NorthwindEntities())
            {
                int cID = context.Categories.Where(c => c.CategoryName.Contains(mpValue)).FirstOrDefault().CategoryID;
                var query = context.Products.Where(p => p.CategoryID == cID).Include(c => c.Category).Include(s => s.Supplier).ToList();
                return View("Index", query);
            }
        }
    }
}
