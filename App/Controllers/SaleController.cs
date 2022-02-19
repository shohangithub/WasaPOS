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
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class SaleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Tbl_User userSession = System.Web.HttpContext.Current.Session["UserSession"] as Tbl_User;
        // GET: Sale
        //Get : Due List Start
        [CheckAuthorization]
        public ActionResult DueList()
        {
            var duelist = db.Customers.Where(c => c.TotalDueAmount > 0).ToList();
            return View(duelist);
        }
        //Get : Due List End
        //Get : Sale List Start
        [CheckAuthorization]
        public ActionResult SaleList()
        {
            var salelist = db.Sales.Where(c => c.SaleDate == DateTime.Today && c.IsDeleted==false).Include(s => s.Customer).ToList();
            return View(salelist);
        }
        public ActionResult DateWiseSaleList(string fromdate)
        {
            DateTime sdate = Convert.ToDateTime(fromdate);
            if (fromdate != null)
            {
                var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false)
                    .Include(s => s.Customer).Select(x => new
                    {
                        x.SaleId,
                        x.SaleDate,
                        x.InvoiceId,
                        x.Customer.CustomerName,
                        x.NetTotal,
                        x.CashReceive,
                        x.TSSD,
                        x.TSVat,
                        x.Discount,
                        x.DiscountAmount,
                        x.DueAmount
                    }).ToList();
                var jsonData = new
                {
                    success = true,
                    message = "",
                    salelist = sale,
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else { 
            var salelist = db.Sales.Where(c => c.SaleDate == DateTime.Today && c.IsDeleted == false).Include(s => s.Customer).ToList();
            return View(salelist);
            }
        }
        //Get : Sale List End
        //Get : Sale Due View Page Start
        [CheckAuthorization]
        public ActionResult SaleEntry()
        {
            var cust = db.Customers.Where(c => c.CustomerType == CustomerType.Regular && c.Status == Status.Accepted).ToList();
            ViewData["CustomerId"] = new SelectList(cust.Select(x => new { Value = x.CustomerId, Text = x.CustomerName }).ToList(), "Value", "Text");
            var productionDate = db.Productions.OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            if (productionDate != null)
            {
                ViewBag.ProductionDate = productionDate.ProductionDate.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.ProductionDate = "No Production Date ";
            }
            int InvoiceNo;
            string invoiceno;
            var Infoes = db.Sales.OrderByDescending(s => s.SaleId).FirstOrDefault();

            if (Infoes != null)
            {
                InvoiceNo = Convert.ToInt32(new String(Infoes.InvoiceId.ToCharArray().Where(c => Char.IsDigit(c)).ToArray())) + 1;

                invoiceno = InvoiceNo.ToString("D7");
            }

            else
            {
                invoiceno = "0000001";
            }
            ViewBag.InvoiceId = invoiceno;
            SaleViewModels vm = new SaleViewModels();


            var products = db.Products.ToList();
            foreach (var product in products)
            {
                if (product.ProductName == "Shanti Water 250 ML")
                {
                    vm.Swml250CurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 0.5 L")
                {
                    vm.Sw5mlCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.0 L")
                {
                    vm.Sw1lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.5 L")
                {
                    vm.Sw15lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 2.0 L")
                {
                    vm.Sw2lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 5.0 L")
                {
                    vm.Sw5lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 30TK")
                {
                    vm.Sw20lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 40Tk")
                {
                    vm.Sw20l4CurrentStock = product.CurrentStoke;
                }
            }
            return View(vm);
        }
        //Get : Sale Due View Page End
        //Get : Sale Cash View Page Start
        [CheckAuthorization]
        public ActionResult SaleInCashEntry()
        {
            var cust = db.Customers.Where(c => c.CustomerType == CustomerType.Regular && c.Status == Status.Accepted).ToList();
            ViewData["CustomerId"] = new SelectList(cust.Select(x => new { Value = x.CustomerId, Text = x.CustomerName }).ToList(), "Value", "Text");
            var productionDate = db.Productions.OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            if (productionDate != null)
            {
                ViewBag.ProductionDate = productionDate.ProductionDate.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.ProductionDate = "No Production Date ";
            }
            int InvoiceNo;
            string invoiceno;
            var Infoes = db.Sales.OrderByDescending(s => s.SaleId).FirstOrDefault();

            if (Infoes != null)
            {
                InvoiceNo = Convert.ToInt32(new String(Infoes.InvoiceId.ToCharArray().Where(c => Char.IsDigit(c)).ToArray())) + 1;

                invoiceno = InvoiceNo.ToString("D7");
            }

            else
            {
                invoiceno = "0000001";
            }
            ViewBag.InvoiceId = invoiceno;
            SaleViewModels vm = new SaleViewModels();


            var products = db.Products.ToList();
            foreach (var product in products)
            {
                if (product.ProductName == "Shanti Water 250 ML")
                {
                    vm.Swml250CurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 0.5 L")
                {
                    vm.Sw5mlCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.0 L")
                {
                    vm.Sw1lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.5 L")
                {
                    vm.Sw15lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 2.0 L")
                {
                    vm.Sw2lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 5.0 L")
                {
                    vm.Sw5lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 30TK")
                {
                    vm.Sw20lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 40Tk")
                {
                    vm.Sw20l4CurrentStock = product.CurrentStoke;
                }
            }
            return View(vm);
        }
        //Get : Sale Cash View Page End
        //Post : Sale Create Action Start
        [HttpPost]
        public JsonResult AddSale(SaleViewModels vm)
        {
            string msg;
            long saleId;
            if (SaveSale(vm, out saleId, out msg))
            {
                var jsonData = new { success = true, message = msg };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = msg };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        //Post : Sale Create Action End
        //Post : Sale Create And Print Action Start
        [HttpPost]
        public ActionResult SaveAndPrintSale(SaleViewModels vm)
        {
            string msg;
            long saleId;
            if (SaveSale(vm, out saleId, out msg))
            {
               
                var jsonData = new
                {
                    success = true,
                    Invurl = Url.Action("Invoice", "SalesPrintReport", new { reportType = "pdf", saleId = saleId }),
                    Geturl = Url.Action("Getpass", "SalesPrintReport", new { reportType = "pdf", saleId = saleId }),
                    Chalanurl = Url.Action("Chalan", "SalesPrintReport", new { reportType = "pdf", saleId = saleId }),
                    saleId = saleId,
                    message = msg
                };
                
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = msg };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        //Post : Sale Create And Print Action End
        //Post : Sale Create And Update  Start
        public bool SaveSale(SaleViewModels vm, out long SaleId, out string msg)
        {
            msg = "";
            SaleId = 0;
            var userId = userSession.UserName;
            var productDetails = vm.SaleDetails.Where(x=>x.Quantity > 0).ToList();
            var countproduct = productDetails.Count;
            if (vm.SaleId > 0)
            {
                var salesEdit = db.Sales.Find(vm.SaleId);

                if (salesEdit == null)
                {
                    msg = "no sales found";
                    return false;
                }

                if (vm.CustomerId <= 0)
                {
                    var customer = new Customer
                    {
                        CustomerName = vm.CustomerName,
                        AddressLine1 = vm.AddressLine1,
                        Phone = vm.Phone,
                        CustomerType = CustomerType.ERegular,
                        AddedById = userId,
                        IpAddress = Request.UserHostAddress,
                        Status = Status.Accepted
                    };
                    salesEdit.Customer = customer;
                }
                else
                {
                    salesEdit.CustomerId = vm.CustomerId;
                }

                salesEdit.CashReceive = vm.CashReceive;
                salesEdit.Discount = vm.Discount;
                salesEdit.DiscountAmount = vm.DiscountAmount;
                salesEdit.DueAmount = vm.DueAmount;
                salesEdit.AvgDueAmount = vm.DueAmount / countproduct;
                salesEdit.AvgCashReceive = (vm.CashReceive+vm.ChangeAmount) / countproduct;

                salesEdit.InvoiceId = vm.InvoiceId;
                salesEdit.NetTotal = vm.NetTotal;
                salesEdit.OtherCost = vm.OtherCost;
                salesEdit.STAmount = vm.STAmount;
                salesEdit.TransportCost = vm.TransportCost;
                salesEdit.TSSD = vm.TSSD;
                salesEdit.TSVat = vm.TSVat;
                salesEdit.ModifiedDate = DateTime.Now;
                salesEdit.ModifiedById = userId;
                salesEdit.IpAddress = Request.UserHostAddress;
                salesEdit.Status = Status.Transaction;
                db.Entry(salesEdit).State = EntityState.Modified;

                foreach (var d in vm.SaleDetails)
                {
                   
                    if (d.Quantity == 0 && d.QtyCase == "0" && d.TotalPrice == 0)
                    {
                        var editSaleDetails = db.SaleDetails.FirstOrDefault(x => x.SaleDetailId == d.SaleDetailId);
                        if (editSaleDetails != null)
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductId == editSaleDetails.ProductId);
                            product.CurrentStoke = product.CurrentStoke + editSaleDetails.Quantity;
                            db.Entry(product).State = EntityState.Modified;
                            db.SaleDetails.Remove(editSaleDetails);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                       
                        if (d.SaleDetailId > 0)
                        {
                            var editSaleDetails = db.SaleDetails.FirstOrDefault(x=>x.SaleDetailId == d.SaleDetailId);

                            if (editSaleDetails != null)
                            {
                                var product = db.Products.FirstOrDefault(p => p.ProductId == editSaleDetails.ProductId);

                                product.CurrentStoke = (product.CurrentStoke + editSaleDetails.Quantity) - d.Quantity;
                                editSaleDetails.SaleDetailId = d.SaleDetailId;
                                editSaleDetails.Quantity = d.Quantity;
                                editSaleDetails.QtyCase = d.QtyCase;
                                editSaleDetails.FacePrice = d.FacePrice;
                                editSaleDetails.TotalPrice = d.TotalPrice;
                                editSaleDetails.ModifiedDate = DateTime.Now;
                                editSaleDetails.ModifiedById = userId;
                                editSaleDetails.IpAddress = Request.UserHostAddress;
                                editSaleDetails.Status = Status.Transaction;

                                db.Entry(product).State = EntityState.Modified;
                                db.Entry(editSaleDetails).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductName == d.ProductName);

                            if (product == null)
                            {
                                msg = d.ProductName + "is not in the product table";
                                return false;
                            }

                            var sdp = db.SaleDetails.Where(x => x.SaleId == vm.SaleId && x.ProductId == product.ProductId).ToList();
                            //var countsdp = sdp.Count();

                            if (sdp.Count() > 0)
                            {
                                product.CurrentStoke = product.CurrentStoke + d.Quantity;
                                db.Entry(product).State = EntityState.Modified;
                                db.SaleDetails.RemoveRange(sdp);
                                db.SaveChanges();
                            }

                            if (d.Quantity > 0)
                            {
                                SaleDetail sd = new SaleDetail { ProductId = product.ProductId, Quantity = d.Quantity, QtyCase = d.QtyCase, FacePrice = d.FacePrice, TotalPrice = d.TotalPrice, AddedById = userId, IpAddress = Request.UserHostAddress, Status = Status.Accepted };
                                salesEdit.SaleDetails.Add(sd);

                                product.CurrentStoke = product.CurrentStoke - d.Quantity;
                                db.Entry(product).State = EntityState.Modified;

                            }

                        }

                    }
                }

               
                var dueCustomer = db.Customers.FirstOrDefault(x=>x.CustomerId == vm.CustomerId);
                if (dueCustomer != null)
                {
                    dueCustomer.TotalDueAmount = dueCustomer.TotalDueAmount + vm.DueAmount;
                    db.Entry(dueCustomer).State = EntityState.Modified;
                }
                try
                {
                    if(db.SaveChanges() > 0)
                    {
                        if(AddUpdateSaleStatement(salesEdit.SaleDate) > 0) //Post :Daily  Sales Total Create And Update  Function Call 
                        {
                            DailySaleStatement(salesEdit.SaleDate, salesEdit.InvoiceId, vm.CustomerName);//Post :Daily  Sales Customer Wise Create And Update  Function Call
                        }
                        msg = "Sales update Successfully!";
                        SaleId = vm.SaleId;
                        return true;
                    }
                   
                   
                    return false;
                }
                catch (Exception ex)
                {
                    msg = "Sales update failed!";
                    return false;
                }
            }

            else
            {


                var invoice = db.Sales.Where(s => s.InvoiceId == vm.InvoiceId).Count();
                if (invoice > 0)
                {
                    msg = "duplicate invoice";
                    return false;

                }

                try
                {
                    
                    Sale sale = new Sale
                    {
                        CashReceive = vm.CashReceive,
                        Discount = vm.Discount,
                        DiscountAmount = vm.DiscountAmount,
                        DueAmount = vm.DueAmount,
                        AvgDueAmount= vm.DueAmount / countproduct,
                        AvgCashReceive = (vm.CashReceive + vm.ChangeAmount) / countproduct,

                        InvoiceId = vm.InvoiceId,
                        NetTotal = vm.NetTotal,
                        OtherCost = vm.OtherCost,
                        STAmount = vm.STAmount,
                        TransportCost = vm.TransportCost,
                        TSSD = vm.TSSD,
                        TSVat = vm.TSVat,
                        SaleDate = vm.SaleDate,
                        CustomerId = vm.CustomerId,
                        AddedById = userId,
                        IpAddress = Request.UserHostAddress,
                        Status = Status.Accepted
                    };

                    if (vm.CustomerId <= 0)
                    {
                        var customer = new Customer
                        {
                            CustomerName = vm.CustomerName,
                            AddressLine1 = vm.AddressLine1,
                            Phone = vm.Phone,
                            CustomerType = CustomerType.ERegular,
                            AddedById = userId,
                            IpAddress = Request.UserHostAddress,
                            Status = Status.Accepted
                        };
                        // db.Customers.Add(customer);
                        sale.Customer = customer;
                    }
                    
                   
                    //db.SalesStatements.Add(st);
                    //lastinsetid=
                    foreach (var d in vm.SaleDetails)
                    {
                        var product = db.Products.FirstOrDefault(p => p.ProductName == d.ProductName);

                        if (product == null)
                        {
                            msg = d.ProductName + "is not in the product table";
                            return false;
                            // return Json(new { success = false, message = d.ProductName + "is not in the product table" }, JsonRequestBehavior.AllowGet);
                        }
                        if (d.Quantity > 0) {
                        SaleDetail sd = new SaleDetail { ProductId = product.ProductId, Quantity = d.Quantity, QtyCase = d.QtyCase, FacePrice = d.FacePrice, TotalPrice = d.TotalPrice, AddedById = userId, IpAddress = Request.UserHostAddress, Status = Status.Accepted };
                        sale.SaleDetails.Add(sd);

                        product.CurrentStoke = product.CurrentStoke - d.Quantity;
                        db.Entry(product).State = EntityState.Modified;
                        }

                    }

                    
                    var dueCustomer = db.Customers.Find(vm.CustomerId);
                    if (dueCustomer != null)
                    {
                        dueCustomer.TotalDueAmount = dueCustomer.TotalDueAmount + vm.DueAmount;
                        db.Entry(dueCustomer).State = EntityState.Modified;
                    }

                    db.Sales.Add(sale);
                    if(db.SaveChanges() > 0)
                    {
                        if(AddUpdateSaleStatement(sale.SaleDate) > 0) //Post :Daily  Sales Total Create And Update  Function Call
                        {
                            DailySaleStatement(sale.SaleDate, sale.InvoiceId, vm.CustomerName);//Post :Daily  Sales Customer Wise Create And Update  Function Call
                        }
                        msg = "Sales Added Successfully!";
                        SaleId = sale.SaleId;
                        return true;
                    }
                    return false;

                    //var jsonData = new { success = true, message = "SalesAdded Successfully!" };
                    //return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // add the error log
                    msg = "Sales Add failed!";
                    return false;
                    // var jsonData = new { success = false, message = "Sales Added faield!" };
                    // return Json(jsonData, JsonRequestBehavior.AllowGet);

                }
            }
        }
        //Post : Sale Create And Update  Start
        //Post :Daily  Sales Customer Wise Create And Update  Start
        public void DailySaleStatement(DateTime salesDate,string InvoiceId, string CustomerName)
        {
            int Swml250Quantity = 0, Sw5mlQuantity = 0, Sw1lQuantity = 0, Sw15lQuintity = 0, Sw2lQuintity = 0, Sw5lQuintity = 0, Sw20lQuintity = 0, Sw20l4Quintity = 0;
            decimal Swml250TotalPrice = 0, Sw5mlTotalPrice = 0, Sw1lTotalPrice = 0, Sw15lTotalPrice = 0, Sw2lTotalPrice = 0, Sw5lTotalPrice = 0, Sw20lTotalPrice = 0, Sw20l4TotalPrice = 0;
            
            var ss = db.DailySalesStatements.Where(t => t.ReceieptNo== InvoiceId).FirstOrDefault();

            var userId = userSession.UserName; //User.Identity.GetUserId();
            if (ss == null)
            {
                // now statment in this date add new sales statment
                // get all sales in this salesDate
                var sales = db.Sales.Include(c => c.Customer).Where(s => s.InvoiceId == InvoiceId).Include(s => s.SaleDetails.Select(p => p.Product)).ToList();

                if (sales != null)
                {
                   
                    foreach (var sale in sales)
                    {
                        if (sale.CustomerId >0)
                        {
                            CustomerName = sale.Customer.CustomerName; 
                        }
                       
                        foreach (var saleDetail in sale.SaleDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (saleDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = saleDetail.Quantity;
                                Swml250TotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = saleDetail.Quantity;
                                Sw5mlTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = saleDetail.Quantity;
                                Sw1lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = saleDetail.Quantity;
                                Sw15lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = saleDetail.Quantity;
                                Sw2lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = saleDetail.Quantity;
                                Sw5lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = saleDetail.Quantity;
                                Sw20lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = saleDetail.Quantity;
                                Sw20l4TotalPrice = saleDetail.TotalPrice;
                            }
                        }
                        
                        db.DailySalesStatements.Add(new DailySalesStatement
                        {
                        DailyStatementDate = salesDate,
                            ReceieptNo = InvoiceId,
                            
                            CustomerName= CustomerName,
                            Swml250Quantity = Swml250Quantity,
                            Sw5mlQuantity = Sw5mlQuantity,
                            Sw1lQuantity = Sw1lQuantity,
                            Sw15lQuintity = Sw15lQuintity,
                            Sw2lQuintity = Sw2lQuintity,
                            Sw5lQuintity = Sw5lQuintity,

                            Sw20lQuintity = Sw20lQuintity,
                            Sw20l4Quintity = Sw20l4Quintity,
                            Swml250TotalPrice = Swml250TotalPrice,
                            Sw5mlTotalPrice = Sw5mlTotalPrice,
                            Sw1lTotalPrice = Sw1lTotalPrice,
                            Sw15lTotalPrice = Sw15lTotalPrice,
                            Sw2lTotalPrice = Sw2lTotalPrice,
                            Sw5lTotalPrice = Sw5lTotalPrice,
                            Sw20lTotalPrice = Sw20lTotalPrice,
                            Sw20l4TotalPrice = Sw20l4TotalPrice,
                            Status = Status.Accepted,
                            AddedById = userId,
                            IpAddress = Request.UserHostAddress
                        });
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                //update sales statment
                // get all sales in this salesDate
                var sales = db.Sales.Include(c => c.Customer).Where(s => s.InvoiceId == InvoiceId).Include(s => s.SaleDetails.Select(p => p.Product)).ToList();


                if (sales != null)
                {
                    foreach (var sale in sales)
                    {
                        foreach (var saleDetail in sale.SaleDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (saleDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = saleDetail.Quantity;
                                Swml250TotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = saleDetail.Quantity;
                                Sw5mlTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = saleDetail.Quantity;
                                Sw1lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = saleDetail.Quantity;
                                Sw15lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = saleDetail.Quantity;
                                Sw2lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = saleDetail.Quantity;
                                Sw5lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = saleDetail.Quantity;
                                Sw20lTotalPrice = saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = saleDetail.Quantity;
                                Sw20l4TotalPrice = saleDetail.TotalPrice;
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

                    ss.Swml250TotalPrice = Swml250TotalPrice;
                    ss.Sw5mlTotalPrice = Sw5mlTotalPrice;
                    ss.Sw1lTotalPrice = Sw1lTotalPrice;
                    ss.Sw2lTotalPrice = Sw2lTotalPrice;
                    ss.Sw15lTotalPrice = Sw15lTotalPrice;
                    ss.Sw5lTotalPrice = Sw5lTotalPrice;
                    ss.Sw20lTotalPrice = Sw20lTotalPrice;
                    ss.Sw20l4TotalPrice = Sw20l4TotalPrice;
                    ss.Status = Status.Accepted;
                    ss.ModifiedById = userId;
                    ss.IpAddress = Request.UserHostAddress;
                    db.Entry(ss).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
        //Post :Daily  Sales Customer Wise Create And Update  End
        //Post :Daily  Sales Total Create And Update  Start
        public int AddUpdateSaleStatement(DateTime salesDate)
        { 
            int Swml250Quantity = 0, Sw5mlQuantity = 0, Sw1lQuantity = 0, Sw15lQuintity = 0, Sw2lQuintity = 0, Sw5lQuintity = 0, Sw20lQuintity = 0, Sw20l4Quintity = 0;
            decimal Swml250TotalPrice = 0, Sw5mlTotalPrice = 0, Sw1lTotalPrice = 0, Sw15lTotalPrice = 0, Sw2lTotalPrice = 0, Sw5lTotalPrice = 0, Sw20lTotalPrice = 0, Sw20l4TotalPrice = 0;
            var ss = db.SalesStatements.Where(t => t.StatementDate == salesDate).FirstOrDefault();
            
            var userId = userSession.UserName; //User.Identity.GetUserId();
            if (ss == null)
            {
                // now statment in this date add new sales statment
                // get all sales in this salesDate
                var sales = db.Sales.Where(s => s.SaleDate == salesDate).Include(s => s.SaleDetails.Select(p => p.Product)).ToList();
                
                if (sales != null)
                {
                    foreach (var sale in sales)
                    {
                        foreach (var saleDetail in sale.SaleDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (saleDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = Swml250Quantity + saleDetail.Quantity;
                                Swml250TotalPrice = Swml250TotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = Sw5mlQuantity + saleDetail.Quantity;
                                Sw5mlTotalPrice = Sw5mlTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = Sw1lQuantity + saleDetail.Quantity;
                                Sw1lTotalPrice = Sw1lTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = Sw15lQuintity + saleDetail.Quantity;
                                Sw15lTotalPrice = Sw15lTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = Sw2lQuintity + saleDetail.Quantity;
                                Sw2lTotalPrice = Sw2lTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = Sw5lQuintity + saleDetail.Quantity;
                                Sw5lTotalPrice = Sw5lTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = Sw20lQuintity + saleDetail.Quantity;
                                Sw20lTotalPrice = Sw20lTotalPrice + saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = Sw20l4Quintity + saleDetail.Quantity;
                                Sw20l4TotalPrice = Sw20l4TotalPrice + saleDetail.TotalPrice;
                            }
                        }

                    }
                    db.SalesStatements.Add(new SalesStatement
                        {
                            StatementDate = salesDate,
                            Swml250Quantity = Swml250Quantity,
                            Sw5mlQuantity = Sw5mlQuantity,
                            Sw1lQuantity = Sw1lQuantity,
                            Sw15lQuintity = Sw15lQuintity,
                            Sw2lQuintity = Sw2lQuintity,
                            Sw5lQuintity = Sw5lQuintity,

                            Sw20lQuintity = Sw20lQuintity,
                            Sw20l4Quintity = Sw20l4Quintity,
                            Swml250TotalPrice = Swml250TotalPrice,
                            Sw5mlTotalPrice = Sw5mlTotalPrice,
                            Sw1lTotalPrice = Sw1lTotalPrice,
                            Sw15lTotalPrice = Sw15lTotalPrice,
                            Sw2lTotalPrice= Sw2lTotalPrice,
                            Sw5lTotalPrice = Sw5lTotalPrice,
                            Sw20lTotalPrice = Sw20lTotalPrice,
                            Sw20l4TotalPrice = Sw20l4TotalPrice,
                            Status = Status.Accepted,
                            AddedById = userId,
                            IpAddress = Request.UserHostAddress
                        });
                   return db.SaveChanges();
                }
            }
            else
            {
                //update sales statment
                // get all sales in this salesDate
                var sales = db.Sales.Where(s => s.SaleDate == salesDate).Include(s => s.SaleDetails.Select(p => p.Product)).ToList();
                if (sales != null)
                {
                    foreach (var sale in sales)
                    {
                        foreach (var saleDetail in sale.SaleDetails)
                        {
                            // read all product and quintity then add it to variable then save
                            if (saleDetail.Product.ProductName == "Shanti Water 250 ML")
                            {
                                //if(sales.> )
                                Swml250Quantity = Swml250Quantity+ saleDetail.Quantity;
                                Swml250TotalPrice = Swml250TotalPrice+saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 0.5 L")
                            {
                                Sw5mlQuantity = Sw5mlQuantity+ saleDetail.Quantity;
                                Sw5mlTotalPrice = Sw5mlTotalPrice+ saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.0 L")
                            {
                                Sw1lQuantity = Sw1lQuantity+ saleDetail.Quantity;
                                Sw1lTotalPrice = Sw1lTotalPrice+saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 1.5 L")
                            {
                                Sw15lQuintity = Sw15lQuintity+saleDetail.Quantity;
                                Sw15lTotalPrice = Sw15lTotalPrice+saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 2.0 L")
                            {
                                Sw2lQuintity = Sw2lQuintity+ saleDetail.Quantity;
                                Sw2lTotalPrice = Sw2lTotalPrice+ saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 5.0 L")
                            {
                                Sw5lQuintity = Sw5lQuintity+ saleDetail.Quantity;
                                Sw5lTotalPrice = Sw5lTotalPrice+ saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 30TK")
                            {
                                Sw20lQuintity = Sw20lQuintity+ saleDetail.Quantity;
                                Sw20lTotalPrice = Sw20lTotalPrice+saleDetail.TotalPrice;
                            }
                            else if (saleDetail.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                            {
                                Sw20l4Quintity = Sw20l4Quintity+ saleDetail.Quantity;
                                Sw20l4TotalPrice = Sw20l4TotalPrice+ saleDetail.TotalPrice;
                            }

                        }
                    }
                    ss.Swml250Quantity = Swml250Quantity;
                    ss.Sw5mlQuantity =  Sw5mlQuantity;
                    ss.Sw1lQuantity = Sw1lQuantity;
                    ss.Sw15lQuintity = Sw15lQuintity;
                    ss.Sw2lQuintity = Sw2lQuintity;
                    ss.Sw5lQuintity =Sw5lQuintity;
                    ss.Sw20lQuintity =  Sw20lQuintity;
                    ss.Sw20l4Quintity =  Sw20l4Quintity;

                    ss.Swml250TotalPrice = Swml250TotalPrice;
                    ss.Sw5mlTotalPrice =  Sw5mlTotalPrice;
                    ss.Sw1lTotalPrice =  Sw1lTotalPrice;
                    ss.Sw2lTotalPrice =  Sw2lTotalPrice;
                    ss.Sw15lTotalPrice = Sw15lTotalPrice;
                    ss.Sw5lTotalPrice =  Sw5lTotalPrice;
                    ss.Sw20lTotalPrice =  Sw20lTotalPrice;
                    ss.Sw20l4TotalPrice = Sw20l4TotalPrice;
                    ss.Status = Status.Accepted;
                    ss.ModifiedById = userId;
                    ss.IpAddress = Request.UserHostAddress;
                    db.Entry(ss).State = EntityState.Modified;
                   return db.SaveChanges();
                }
            }
            return 0;
        }
        //Post :Daily  Sales Total Create And Update  End
        //Post : Sales Edit  Page Start
        public ActionResult SaleEdit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sales = db.Sales.Include(c => c.Customer).Include(s => s.SaleDetails.Select(p => p.Product)).Where(s => s.SaleId == id).FirstOrDefault();
            ViewBag.ProductionDate = db.Productions.OrderByDescending(p => p.ProductionDate).FirstOrDefault().ProductionDate.ToString("yyyy-MM-dd");
            if (sales == null)
            {
                return HttpNotFound();
            }

            ViewBag.InvoiceId = sales.InvoiceId;
            SaleViewModels vm = new SaleViewModels();
            vm.SaleId = sales.SaleId;
            vm.InvoiceId = sales.InvoiceId;
            vm.AddressLine1 = sales.Customer.AddressLine1;
            vm.CashReceive = sales.CashReceive;
            vm.Discount = sales.Discount;
            vm.DiscountAmount = sales.DiscountAmount;
            vm.DueAmount = sales.DueAmount;
            vm.NetTotal = sales.NetTotal;
            vm.STAmount = sales.STAmount;
            vm.TransportCost = sales.TransportCost;
            vm.TSSD = sales.TSSD;
            vm.TSVat = sales.TSVat;
            vm.Phone = sales.Customer.Phone;
            vm.SaleDate = sales.SaleDate;
        
            foreach (var salesItem in sales.SaleDetails)
            {
                if (salesItem.Product.ProductName == "Shanti Water 250 ML")
                {
                    vm.Swml250SaleDetailId = salesItem.SaleDetailId;
                    vm.Swml250Quintity = salesItem.Quantity;
                    vm.Swml250QtyCase = salesItem.QtyCase;
                    vm.Swml250FacePrice = salesItem.FacePrice;
                    vm.Swml250TotalPrice = salesItem.TotalPrice;
                   
                }
                else if (salesItem.Product.ProductName == "Shanti Water 0.5 L")
                {
                    vm.Sw5mlSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw5mlQuintity = salesItem.Quantity;
                    vm.Sw5mlQtyCase = salesItem.QtyCase;
                    vm.Sw5mlFacePrice = salesItem.FacePrice;
                    vm.Sw5mlTotalPrice = salesItem.TotalPrice;
                   
                }
                else if (salesItem.Product.ProductName == "Shanti Water 1.0 L")
                {
                    vm.Sw1lSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw1lQuintity = salesItem.Quantity;
                    vm.Sw1lQtyCase = salesItem.QtyCase;
                    vm.Sw1lFacePrice = salesItem.FacePrice;
                    vm.Sw1lTotalPrice = salesItem.TotalPrice;
                    

                }
                else if (salesItem.Product.ProductName == "Shanti Water 1.5 L")
                {
                    vm.Sw15lSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw15lQuintity = salesItem.Quantity;
                    vm.Sw15lQtyCase = salesItem.QtyCase;
                    vm.Sw15lFacePrice = salesItem.FacePrice;
                    vm.Sw15lTotalPrice = salesItem.TotalPrice;
                    
                }
                else if (salesItem.Product.ProductName == "Shanti Water 2.0 L")
                {
                    vm.Sw2lSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw2lQuintity = salesItem.Quantity;
                    vm.Sw2lQtyCase = salesItem.QtyCase;
                    vm.Sw2lFacePrice = salesItem.FacePrice;
                    vm.Sw2lTotalPrice = salesItem.TotalPrice;
                    
                }
                else if (salesItem.Product.ProductName == "Shanti Water 5.0 L")
                {
                    vm.Sw5lSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw5lQuintity = salesItem.Quantity;
                    vm.Sw5lQtyCase = salesItem.QtyCase;
                    vm.Sw5lFacePrice = salesItem.FacePrice;
                    vm.Sw5lTotalPrice = salesItem.TotalPrice;
                   
                }
                else if (salesItem.Product.ProductName == "Shanti Water 20.0 L 30TK")
                {
                    vm.Sw20lSaleDetailId = salesItem.SaleDetailId;
                    vm.Sw20lQuintity = salesItem.Quantity;
                    vm.Sw20lQtyCase = salesItem.QtyCase;
                    vm.Sw20lFacePrice = salesItem.FacePrice;
                    vm.Sw20lTotalPrice = salesItem.TotalPrice;
                   
                }
                else if (salesItem.Product.ProductName == "Shanti Water 20.0 L 40Tk")
                {
                    vm.Sw20l4SaleDetailId = salesItem.SaleDetailId;
                    vm.Sw20l4Quintity = salesItem.Quantity;
                    vm.Sw20l4QtyCase = salesItem.QtyCase;
                    vm.Sw20l4FacePrice = salesItem.FacePrice;
                    vm.Sw20l4TotalPrice = salesItem.TotalPrice;
                   
                }
            }
            var products = db.Products.ToList();
            foreach (var product in products)
            {
                if (product.ProductName == "Shanti Water 250 ML")
                {
                    vm.Swml250CurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 0.5 L")
                {
                    vm.Sw5mlCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.0 L")
                {
                    vm.Sw1lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 1.5 L")
                {
                    vm.Sw15lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 2.0 L")
                {
                    vm.Sw2lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 5.0 L")
                {
                    vm.Sw5lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 30TK")
                {
                    vm.Sw20lCurrentStock = product.CurrentStoke;
                }
                else if (product.ProductName == "Shanti Water 20.0 L 40Tk")
                {
                    vm.Sw20l4CurrentStock = product.CurrentStoke;
                }
            }
            ViewData["CustomerId"] = new SelectList(db.Customers.Select(x => new { Value = x.CustomerId, Text = x.CustomerName }).ToList(), "Value", "Text", sales.CustomerId);
            return View(vm);
        }
        //Post : Sales Edit  Page End
        //Post : Sales Delete  Start
        public JsonResult DeleteSale(long id)
        {
            var userId = userSession.UserName; //User.Identity.GetUserId();
            var sales = db.Sales.Include(c => c.Customer).Include(s => s.SaleDetails.Select(p => p.Product)).Where(s => s.SaleId == id).FirstOrDefault();


            sales.IsDeleted = true;
            sales.Status = Status.Rejected;
            sales.ModifiedById = userId;
            sales.IpAddress = Request.UserHostAddress;
            foreach (var salesItem in sales.SaleDetails)
            {
                salesItem.IsDeleted = true;
                salesItem.Status = Status.Rejected;
                salesItem.ModifiedById = userId;
                salesItem.IpAddress = Request.UserHostAddress;
                db.Entry(salesItem).State = EntityState.Modified;


                var product = db.Products.Where(p => p.ProductName == salesItem.Product.ProductName).FirstOrDefault();
                product.CurrentStoke = product.CurrentStoke + salesItem.Quantity;
                db.Entry(product).State = EntityState.Modified;


            }
            db.Entry(sales).State = EntityState.Modified;
            db.SaveChanges();
            var jsonData = new { success = true, message = "Sale  Deleted Successfully!" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        //Post : Sales Delete  End
        //Get : Sales Due Collection Page Start
        [CheckAuthorization]
        public ActionResult DueCollection()
        {
            int TrNo;
            string trno;
            var Infoes = db.DueTransactions.OrderByDescending(s => s.DueTransactionId).FirstOrDefault();

            if (Infoes != null)
            {
                TrNo = Convert.ToInt32(new String(Infoes.DueTR_Id.ToCharArray().Where(c => Char.IsDigit(c)).ToArray())) + 1;

                trno = "TR" + TrNo.ToString("D7");
            }

            else
            {
                trno = "TR0000001";
            }
            ViewBag.DueTR_Id = trno;
            var sale = db.Sales.Where(c => c.DueAmount >0 && c.Status!= Status.Paid).ToList();
            ViewData["SaleId"] = new SelectList(sale.Select(x => new { Value = x.SaleId, Text = x.InvoiceId }).ToList(), "Value", "Text");
            var paidList = db.DueTransactions.Include(c => c.Customer).ToList();

            return View(paidList);
        }
        //Get : Sales Due Collection Page End
        //Get : Sales Due Collection List Start
        public ActionResult DueCollectionList(long id)
        {
            var paidList = db.Sales.Include(c => c.Customer).Where(x=> x.CustomerId==id && x.DueAmount>0).OrderBy(x=> x.SaleDate).ToList();
            return View(paidList);
        }
        //Get : Sales Due Collection List End
        //Post : Sales Due Collection Create Start
        public JsonResult Addtransaction(DueTransaction tr, long CustomerId, string DueTR_Id, DateTime PaymentDate, string TrType, decimal PaidAmount,long SaleId)
        {

            tr.DueTR_Id = DueTR_Id;
            tr.CustomerId = CustomerId;
            tr.PaymentDate = PaymentDate;
            if (TrType == "Receive")
            {
                tr.InAmount = PaidAmount;
                tr.OutAmount = 0;
            }
            else
            {
                tr.OutAmount = PaidAmount;
                tr.InAmount = 0;
            }
            tr.TrType = TrType;
            var dueCustomer = db.Customers.Find(tr.CustomerId);
            if (dueCustomer != null)
            {
                dueCustomer.TotalDueAmount = dueCustomer.TotalDueAmount - tr.InAmount;
                db.Entry(dueCustomer).State = EntityState.Modified;
            }
            var dueSale = db.Sales.Find(SaleId);
            if (dueSale != null)
            {
                dueSale.Status = Status.Paid;
                db.Entry(dueSale).State = EntityState.Modified;
            }
            var userId = userSession.UserName; //User.Identity.GetUserId();
            tr.AddedById = userId;
            tr.IpAddress = Request.UserHostAddress;
            db.DueTransactions.Add(tr);
            db.SaveChanges();
            var jsonData = new { success = true, message = "Transaction Added Successfully!" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        //Post : Sales Due Collection Create End
        //Post : Sales Record Search  Date wise
        [CheckAuthorization]
        public ActionResult SalesRecord()
        {
            return View();
        }
        public PartialViewResult SalesRecordList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            var sale = db.Sales.Where(c => c.SaleDate >= sdate && c.SaleDate <= edate && c.IsDeleted == false).Include(c => c.Customer).ToList();

            return this.PartialView("SalesRecordList", sale);

        } //Post : Sales Record Search  Date wise End
        [CheckAuthorization]
        public ActionResult SalesRecords()
        {
            return View();
        }

        public PartialViewResult SalesRecordsList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var sale = db.Sales.Where(c => c.SaleDate >= sdate && c.SaleDate <= edate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("SalesRecordsList", sale);

        }
        [CheckAuthorization]
        public ActionResult DailyVatRecord()
        {
            return View();
        }
        public PartialViewResult DailyVatRecordList(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatRecordList", sale);

        }

        //[CheckAuthorization]
        public ActionResult DailyBottleVatRecord()
        {
            return View();
        }
        public PartialViewResult DailyBottleVatRecordList(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatBottleRecordList", sale);

        }

        //[CheckAuthorization]
        public ActionResult DailyJarVatRecord()
        {
            return View();
        }
        public PartialViewResult DailyJarVatRecordList(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatJarRecordList", sale);

        }
        [CheckAuthorization]
        public ActionResult MonthlyVatRecord()
        {
           var year = db.Sales.Select(x => new { Value = x.SaleDate.Year, Text = x.SaleDate.Year }).Distinct();
            ViewBag.Year = new SelectList(year, "Value", "Text");

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Jan", Value = "1" });
            list.Add(new SelectListItem { Text = "Feb", Value = "2" });
            list.Add(new SelectListItem { Text = "Mar", Value = "3" });
            list.Add(new SelectListItem { Text = "Apr", Value = "4" });
            list.Add(new SelectListItem { Text = "May", Value = "5" });
            list.Add(new SelectListItem { Text = "Jun", Value = "6" });
            list.Add(new SelectListItem { Text = "Jul", Value = "7" });
            list.Add(new SelectListItem { Text = "Aug", Value = "8" });
            list.Add(new SelectListItem { Text = "Sep", Value = "9" });
            list.Add(new SelectListItem { Text = "Oct", Value = "10" });
            list.Add(new SelectListItem { Text = "Nov", Value = "11" });
            list.Add(new SelectListItem { Text = "Dec", Value = "12" });
            ViewBag.Month = new SelectList(list, "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult YearlyVatRecord()
        {
            var year = db.Sales.Select(x => new { Value = x.SaleDate.Year, Text = x.SaleDate.Year }).Distinct();
            ViewBag.Year = new SelectList(year, "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult DailyVatRecords()
        {
            return View();
        }
        public PartialViewResult DailyVatRecordLists(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatRecordLists", sale);

        }
        public ActionResult DailyBottleVatRecords()
        {
            return View();
        }
        public PartialViewResult DailyBottleVatRecordLists(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatBottleRecordLists", sale);

        }
        //[CheckAuthorization]
        public ActionResult DailyJarVatRecords()
        {
            return View();
        }
        public PartialViewResult DailyJarVatRecordLists(string date)
        {
            ViewBag.date = date;
            DateTime sdate = Convert.ToDateTime(date);
            var sale = db.Sales.Where(c => c.SaleDate == sdate && c.IsDeleted == false).Include(c => c.Customer).ToList();
            ViewBag.VatAmount = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            return this.PartialView("VatJarRecordLists", sale);

        }

        [CheckAuthorization]
        public ActionResult MonthlyVatRecords()
        {
            var year = db.Sales.Select(x => new { Value = x.SaleDate.Year, Text = x.SaleDate.Year }).Distinct();
            ViewBag.Year = new SelectList(year, "Value", "Text");

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Jan", Value = "1" });
            list.Add(new SelectListItem { Text = "Feb", Value = "2" });
            list.Add(new SelectListItem { Text = "Mar", Value = "3" });
            list.Add(new SelectListItem { Text = "Apr", Value = "4" });
            list.Add(new SelectListItem { Text = "May", Value = "5" });
            list.Add(new SelectListItem { Text = "Jun", Value = "6" });
            list.Add(new SelectListItem { Text = "Jul", Value = "7" });
            list.Add(new SelectListItem { Text = "Aug", Value = "8" });
            list.Add(new SelectListItem { Text = "Sep", Value = "9" });
            list.Add(new SelectListItem { Text = "Oct", Value = "10" });
            list.Add(new SelectListItem { Text = "Nov", Value = "11" });
            list.Add(new SelectListItem { Text = "Dec", Value = "12" });
            ViewBag.Month = new SelectList(list, "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult YearlyVatRecords()
        {
            var year = db.Sales.Select(x => new { Value = x.SaleDate.Year, Text = x.SaleDate.Year }).Distinct();
            ViewBag.Year = new SelectList(year, "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleAndJarSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleAndJarSaleWithService()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleAndJarSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult BottleSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult JarSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult JarSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyBottleAndJarSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyBottleAndJarSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyBottleSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyBottleSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyJarSale()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult MonthlyJarSales()
        {
            return View();
        }
        public PartialViewResult BottleAndJarSalesList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var sale = db.Sales.Where(c => c.SaleDate >= sdate && c.SaleDate <= edate).Include(c => c.Customer).ToList();

            return this.PartialView("SalesRecordsList", sale);

        }
        [CheckAuthorization]
        public ActionResult InvoiceSearch()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult DueAllRecords()
        {
            return View();
        }
        public PartialViewResult DueAllRecordsList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var all = db.DueTransactions.Where(c => c.PaymentDate >= sdate && c.PaymentDate <= edate).Include(c => c.Customer).ToList();

            return this.PartialView("DueAllRecordsList", all);

        }
        [CheckAuthorization]
        public ActionResult DueCollectionRecords()
        {
            var cust = db.Customers.Where(c => c.CustomerType == CustomerType.Regular).ToList();
            ViewData["CustomerId"] = new SelectList(cust.Select(x => new { Value = x.CustomerId, Text = x.CustomerName }).ToList(), "Value", "Text");
            return View();
        }
        public PartialViewResult DueCollectionRecordsList(long CustomerId, string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            ViewBag.ac = CustomerId;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var all = db.DueTransactions.Where(c => c.CustomerId == CustomerId && c.PaymentDate >= sdate && c.PaymentDate <= edate).Include(c => c.Customer).ToList();

            return this.PartialView("DueCollectionRecordsList", all);

        }
        [CheckAuthorization]
        public ActionResult GatePassRecords()
        {
            return View();
        }
        public PartialViewResult GatePassRecordsList(string fromdate, string todate)
        {
            ViewBag.date = fromdate;
            ViewBag.tdate = todate;
            //DateTime sdate = Convert.ToDateTime(Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy"));
            //DateTime edate = Convert.ToDateTime(Convert.ToDateTime(todate).ToString("dd/MM/yyyy"));
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            var sale = db.Sales.Where(c => c.SaleDate >= sdate && c.SaleDate <= edate && c.IsDeleted == false).Include(c => c.Customer).Include(s => s.SaleDetails.Select(p => p.Product)).ToList();

            return this.PartialView("SalesRecordsList", sale);

        }
        [CheckAuthorization]
        public ActionResult VatEntry()
        {
            return View(db.VatEntrys.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult UpdateVat(VatEntry vatEntry)
        {
                var existvat = db.VatEntrys.FirstOrDefault(x => x.VatId == vatEntry.VatId);
                existvat.VatAmount = vatEntry.VatAmount;
                var userId = userSession.UserName; //User.Identity.GetUserId();
                existvat.ModifiedById = userId;
                existvat.ModifiedDate = DateTime.Now;
                existvat.IpAddress = Request.UserHostAddress;
                db.Entry(existvat).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("VatEntry");
            
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