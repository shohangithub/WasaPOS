using App.Helper;
using Data;
using Microsoft.AspNet.Identity;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Tbl_User userSession = System.Web.HttpContext.Current.Session["UserSession"] as Tbl_User;
        // GET: Product
        [CheckAuthorization]
        public ActionResult ProductStock()
        {
            return View(db.Products.ToList());
        }
        [CheckAuthorization]
        public ActionResult NewProduct()
        {
            return View(db.Products.ToList());
        }
        public ActionResult ProductList()
        {
            return PartialView(db.Products.ToList());
        }
        public JsonResult AddProduct(Product p,string product_name,string size,string unit,
            decimal cost_price, decimal sale_price,decimal sd, decimal vat)
        {
            try {
                var userId = userSession.UserName;// User.Identity.GetUserId();
                p.AddedById = userId;
                p.ProductName = product_name;
                p.Size = size;
                p.Uint = unit;
                p.CostPrice = cost_price;
                p.FacePrice = sale_price;
                p.VatInParcent = vat;
                p.SDInParcent = sd;
                db.Products.Add(p);
                db.SaveChangesAsync();
                var jsonData = new { success = true, message = "Product Added Successfully!" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var jsonData = new { success = false, message = "Product Added faield!" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
    }
}