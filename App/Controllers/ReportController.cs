using App.Helper;
using App.Models;
using Data;
using Microsoft.AspNet.Identity;
using Model;
using Model.Entities;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
namespace App.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        [CheckAuthorization]
        public ActionResult WaterProduction()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult WaterSales()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult WaterSalesCustomer()
        {
            var cust = db.Customers.Where(c => c.CustomerType == CustomerType.Regular && c.Status == Status.Accepted).ToList();
            ViewData["CustomerId"] = new SelectList(cust.Select(x => new { Value = x.CustomerId, Text = x.CustomerName }).ToList(), "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult WaterSalesUser()
        {
            ViewData["UserId"] = new SelectList(db.Tbl_User.Select(x => new { Value = x.UserName, Text = x.UserName }).ToList(), "Value", "Text");
            return View();
        }
        [CheckAuthorization]
        public ActionResult DailyBottleSale()
        {
           
            return View();
        }
        [CheckAuthorization]
        public ActionResult DailyBottleSales()
        {

            return View();
        }
        [CheckAuthorization]
        public ActionResult DailyJarSale()
        {

            return View();
        }
        [CheckAuthorization]
        public ActionResult DailyJarSales()
        {

            return View();
        }
    }
}