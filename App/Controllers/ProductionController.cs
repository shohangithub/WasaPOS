using App.Helper;
using Data;
using Microsoft.AspNet.Identity;
using Model;
using Model.Entities;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{

    public class ProductionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Tbl_User userSession = System.Web.HttpContext.Current.Session["UserSession"] as Tbl_User;
        // GET: Production
        [CheckAuthorization]
        public ActionResult Index()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult NewProduction()
        {
            // ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { Value = x.BranchId, Text = x.BranchName }).ToList(), "Value", "Text");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }
        [HttpPost]
        public JsonResult AddProduction(ProductionViewModel vm)
        {
            try
            {
                var userId = userSession.UserName; //User.Identity.GetUserId();
                Production production = new Production { ProductionDate = vm.ProductionDate, ProductionShift = vm.ProductionShift , AddedById=userId, IpAddress=Request.UserHostAddress, Status= Status.Accepted };
                foreach (var d in vm.ProductionDetails)
                {
                    var product = db.Products.Where(p => p.ProductName == d.ProductName).FirstOrDefault();

                    if (product == null)
                    {                     
                        return Json(new { success = false, message = d.ProductName+ "is not in the product table" }, JsonRequestBehavior.AllowGet);
                    }
                    ProductionDetail pd = new ProductionDetail { ProductId = product.ProductId, Quantity = d.Quantity, QtyCase = d.QtyCase, Litter=d.Litter, AddedById = userId, IpAddress = Request.UserHostAddress, Status = Status.Accepted };
                    production.ProductionDetails.Add(pd);

                    product.CurrentStoke = product.CurrentStoke + d.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                }
                db.Productions.Add(production);
                db.SaveChanges();
                
                var jsonData = new { success = true, message = "Production Added Successfully!" };
                AddUpdateProductionStatement(production.ProductionDate);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {// add the error log
                var jsonData = new { success = false, message = "Production Added faield!" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);

            }
           
        }
        public void AddUpdateProductionStatement(DateTime productionDate)
        {
            int Swml250Quantity = 0, Sw5mlQuantity = 0, Sw1lQuantity = 0, Sw15lQuintity = 0, Sw2lQuintity = 0, Sw5lQuintity = 0, Sw20lQuintity = 0, Sw20l4Quintity = 0;
            
            var ss = db.ProductionStatements.Where(t => t.ProductionDate == productionDate).FirstOrDefault();

            var userId = userSession.UserName; //User.Identity.GetUserId();
            if (ss == null)
            {
                // now statment in this date add new sales statment
                // get all sales in this salesDate
                var productions = db.Productions.Where(s => s.ProductionDate == productionDate).Include(s => s.ProductionDetails.Select(p => p.Product)).ToList();

                if (productions != null)
                {
                    foreach (var production in productions)
                    {
                        foreach (var productionDetail in production.ProductionDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (productionDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = Swml250Quantity + productionDetail.Quantity;
                                }
                            else if (productionDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = Sw5mlQuantity + productionDetail.Quantity;
                             }
                            else if (productionDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = Sw1lQuantity + productionDetail.Quantity;
                                
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = Sw15lQuintity + productionDetail.Quantity;
                                
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = Sw2lQuintity + productionDetail.Quantity;
                               
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = Sw5lQuintity + productionDetail.Quantity;
                                
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = Sw20lQuintity + productionDetail.Quantity;
                                
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = Sw20l4Quintity + productionDetail.Quantity;
                                
                            }
                        }

                    }
                    db.ProductionStatements.Add(new ProductionStatement
                    {
                        ProductionDate = productionDate,
                        Swml250Quantity = Swml250Quantity,
                        Sw5mlQuantity = Sw5mlQuantity,
                        Sw1lQuantity = Sw1lQuantity,
                        Sw15lQuintity = Sw15lQuintity,
                        Sw2lQuintity = Sw2lQuintity,
                        Sw5lQuintity = Sw5lQuintity,
                        Sw20lQuintity = Sw20lQuintity,
                        Sw20l4Quintity = Sw20l4Quintity,
                        
                        Status = Status.Accepted,
                        AddedById = userId,
                        IpAddress = Request.UserHostAddress
                    });
                    db.SaveChanges();
                }
            }
            else
            {
                //update sales statment
                // get all sales in this salesDate
                var productions = db.Productions.Where(s => s.ProductionDate == productionDate).Include(s => s.ProductionDetails.Select(p => p.Product)).ToList();

                if (productions != null)
                {
                    foreach (var production in productions)
                    {
                        foreach (var productionDetail in production.ProductionDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (productionDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = Swml250Quantity + productionDetail.Quantity;
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = Sw5mlQuantity + productionDetail.Quantity;
                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = Sw1lQuantity + productionDetail.Quantity;

                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = Sw15lQuintity + productionDetail.Quantity;

                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = Sw2lQuintity + productionDetail.Quantity;

                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = Sw5lQuintity + productionDetail.Quantity;

                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = Sw20lQuintity + productionDetail.Quantity;

                            }
                            else if (productionDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = Sw20l4Quintity + productionDetail.Quantity;

                            }
                        }

                    }
                    ss.Swml250Quantity = Swml250Quantity;
                    ss.Sw5mlQuantity = Sw5mlQuantity;
                    ss.Sw1lQuantity = Sw1lQuantity;
                    ss.Sw15lQuintity = Sw15lQuintity;
                    ss.Sw2lQuintity = Sw2lQuintity;
                    ss.Sw5lQuintity = Sw5lQuintity;
                    ss.Sw20lQuintity = Sw20lQuintity;
                    ss.Sw20l4Quintity = Sw20l4Quintity;
                    ss.Status = Status.Accepted;
                    ss.ModifiedById = userId;
                    ss.IpAddress = Request.UserHostAddress;
                    db.Entry(ss).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
        [CheckAuthorization]
        public ActionResult BottleAndJarProduction()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleProduction()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult JarProduction()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult ProductionReport()
        {
            return View();
        }
        public PartialViewResult ProductionReportList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var sale = db.Productions.Where(c => c.ProductionDate >= sdate && c.ProductionDate <= edate).Include(c => c.ProductionDetails).ToList();

            return this.PartialView("ProductionReportList", sale);

        }
    }
}