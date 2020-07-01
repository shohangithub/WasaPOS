using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using Model.Entities;
using Microsoft.AspNet.Identity;
using Model;
using System;
using App.Helper;

namespace App.Controllers
{
   
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Tbl_User userSession = System.Web.HttpContext.Current.Session["UserSession"] as Tbl_User;
        private string CustomerName, AddressLine1, Phone;
        private decimal TotalDueAmount, DueAmount;
       
        private long CustomerId;
        // GET: Customer
        public async Task<ActionResult> CustomerList()
        {
            return PartialView(await db.Customers.Where(c=>c.CustomerType== CustomerType.Regular && c.Status == Status.Accepted).ToListAsync());
        }
        [CheckAuthorization]
        public async Task<ActionResult> NewCustomer()
        {
           
            int CustomerUId;
            string customeruid;
            var Infoes = db.Customers.Where(c => c.CustomerType == CustomerType.Regular && c.CustomerUId!="").OrderByDescending(c => c.CustomerId).FirstOrDefault();
            if (Infoes != null)
            {
                CustomerUId = Convert.ToInt32(new String(Infoes.CustomerUId.ToCharArray().Where(c => Char.IsDigit(c)).ToArray())) + 1;

                customeruid = "C" + CustomerUId.ToString("D5");
            }

            else
            {
                customeruid = "C0001";
            }
            ViewBag.CustomerUId = customeruid;
            return View(await db.Customers.Where(c => c.CustomerType == CustomerType.Regular && c.Status ==Status.Accepted).ToListAsync());
        }
        public JsonResult AddCustomer(Customer c,string CustomerUId, string customer_name,string contact_persion, string address,string district,string phone, string email)
        {
            var userId = userSession.UserName; //User.Identity.GetUserId();
            c.AddedById = userId;
            c.IpAddress = Request.UserHostAddress;
            c.CustomerUId = CustomerUId;
            c.CustomerName = customer_name;
            c.ContactPerson = contact_persion;
            c.AddressLine1 = address;
            c.District = district;
            c.Phone = phone;
            c.Email = email;
            c.CustomerType = CustomerType.Regular;
            c.Status = Status.Accepted;
            db.Customers.Add(c);
            db.SaveChangesAsync();
            var jsonData = new { success = true, message = "Customer Added Successfully!" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getByID(int? id)
        {
            var sc = db.Customers.Where(c => c.CustomerId == id)
                .Select(x => new { scid = x.CustomerId, CustomerName = x.CustomerName, contact_persion = x.ContactPerson, address = x.AddressLine1, district = x.District, phone = x.Phone, email = x.Email }).FirstOrDefault();
            return Json(sc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCustomer(Customer c,long CustomerId, string customer_name, string contact_persion, string address, string district, string phone, string email)
        {
            var customer = db.Customers.Find(c.CustomerId);
            var userId = userSession.UserName; //User.Identity.GetUserId();
            customer.ModifiedById = userId;
            customer.IpAddress = Request.UserHostAddress;
            customer.CustomerId = CustomerId;
            customer.CustomerName = customer_name;
            customer.ContactPerson = contact_persion;
            customer.AddressLine1 = address;
            customer.District = district;
            customer.Phone = phone;
            customer.Email = email;

            TryUpdateModel(customer);
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChangesAsync();
            var jsonData = new { success = true, message = "Customer Update Successfully!" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int? id)
        {
            var userId = userSession.UserName; //User.Identity.GetUserId();
            var CustomerId = id;
            Customer p = db.Customers.Find(CustomerId);
            p.IsDeleted = true;
            p.Status = Status.Rejected;
            p.ModifiedById = userId;
            p.IpAddress = Request.UserHostAddress;
            TryUpdateModel(p);
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            var jsonData = new { success = true, message = "Customer  Deleted Successfully!" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerIfon(long CustomerId)
        {
            var info = (from item in db.Customers where item.CustomerId == CustomerId  select item).ToList();
            if (info.Count() > 0)
            {
                foreach (var item in info)
                {
                    CustomerId = item.CustomerId;
                    AddressLine1 = item.AddressLine1;
                    Phone = item.Phone;
                    TotalDueAmount = item.TotalDueAmount;
                }
                
                var jsonData = new
                {
                    success = true,
                    message = "",
                    CustomerId = CustomerId,
                    AddressLine1 = AddressLine1,
                    Phone = Phone,

                   
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = "Customer name Not Found! Please Try Again." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult GetDueCustomer(string CustomerUId)
        {
            var info = (from item in db.Customers where item.CustomerType == CustomerType.Regular && item.CustomerUId == CustomerUId && item.Status == Status.Accepted select item).ToList();
            if (info.Count() > 0)
            {
                foreach (var item in info)
                {
                    CustomerName = item.CustomerName;
                    CustomerId = item.CustomerId;
                    AddressLine1 = item.AddressLine1;
                    Phone = item.Phone;
                    TotalDueAmount = item.TotalDueAmount;
                }

                var jsonData = new
                {
                    success = true,
                    message = "",
                    CustomerId = CustomerId,
                    CustomerName = CustomerName,
                    AddressLine1 = AddressLine1,
                    Phone = Phone,
                    TotalDueAmount = TotalDueAmount

                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = "Customer name Not Found! Please Try Again." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult GetDueInvoice(long SaleId)
        {
            var info = (from item in db.Sales where item.DueAmount >0  && item.SaleId == SaleId  select item).ToList();
            if (info.Count() > 0)
            {
                foreach (var item in info)
                {
                    DueAmount = item.DueAmount;
                }

                var jsonData = new
                {
                    success = true,
                    message = "",                   
                    DueAmount = DueAmount

                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = "Invoice Not Found! Please Try Again." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }


        }

    }
}