using App.Helper;
using Data;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace App.Controllers
{
    public class SecurityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SessionalData session = new SessionalData();
        private Tbl_User userSession = System.Web.HttpContext.Current.Session["UserSession"] as Tbl_User;
        private List<ViewMenuPermission> menuList = System.Web.HttpContext.Current.Session["MenuList"] as List<ViewMenuPermission>;
        [AllowAnonymous]
        public ActionResult Login(string errorMsg = "")
        {
            return View(new Tbl_User());
        }

        [HttpPost]
        public ActionResult Login(FormCollection data)
        {
            string user = data["UserName"];
            string password = data["UserPassword"];
          
            
            if (user == "" && password == "") ModelState.AddModelError("", "Enter User ID & Password");
            else if (user == "") ModelState.AddModelError("", "Enter User ID");
            else if (password == "") ModelState.AddModelError("", "Enter Password");
            else if (user != "" && password != "")
            {
                bool result = session.CheckUser(user, password);
                if (result)
                {
                    //var route = System.Web.HttpContext.Current.Session["RouteDirection"].ToString();
                    //if (route != null)
                    //  {

                    //  }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //if(user=="admin" && password=="1234")
                    //return RedirectToAction("Index", "Home");
                    //else
                     ModelState.AddModelError("", "Login details are wrong !");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong !");
            }
            return View();

        }
        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();

            return RedirectToAction("Login", "Security");
        }

        [CheckAuthorization]
        public ActionResult UserRole()
        {
            return View(db.Tbl_UserRole.ToList());
        }


        [HttpPost]
        public JsonResult AddUserRole(Tbl_UserRole userRoll)
        {
            var ExistingRole = (from item in db.Tbl_UserRole where item.RoleName == userRoll.RoleName select item).ToList();

            if (ExistingRole.Count > 0)
            {
                // show message
                var jsonData = new { success = true, message = "This Role Already Assigned !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                userRoll.AddBy = "Admin";
                userRoll.AddDate = DateTime.Now;

                db.Tbl_UserRole.Add(userRoll);
                db.SaveChanges();
                var jsonData1 = new { success = true, message = "User Role Created Successfully." };
                return Json(jsonData1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserRoleList()
        {
            return PartialView(db.Tbl_UserRole.ToList());
        }

        public ActionResult UpdateUserRole(Tbl_UserRole userRoll, int RoleId, string RoleName)
        {
            userRoll = db.Tbl_UserRole.FirstOrDefault(x => x.RoleId == RoleId);
            userRoll.RoleName = RoleName;
            userRoll.EditBy = "Admin";
            userRoll.EditDate = DateTime.Now;
            db.Entry(userRoll).State = EntityState.Modified;
            db.SaveChanges();
            var jsonData = new { success = true, message = "User Role Updated Successfully" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteUserRole(int RoleId)
        {
            var existingdata = (from item in db.Tbl_User where item.RoleId == RoleId select item).ToList();

            if (existingdata.Count > 0)
            {
                // show message
                var jsonData = new { success = true, message = "Can not delete the Role. already use in." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = db.Tbl_UserRole.FirstOrDefault(x => x.RoleId == RoleId);
                db.Tbl_UserRole.Remove(data);
                db.SaveChanges();
                var jsonData = new { success = true, message = "Deleted Successfully" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        //---------------------------User----------------------------------//
        public ActionResult ChangePassword()
        {
            return View(db.Tbl_User.Where(x => x.UserId == userSession.UserId).ToList());
        }
        public ActionResult UpdateUserIndivisual(Tbl_User user, int UserId,string UserPassword,string Oldpassword)
        {
            var newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(UserPassword.Trim(), "md5"); //(for encryption)
            var oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Oldpassword.Trim(), "md5"); //(for encryption)

            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(Oldpassword.Trim(), "md5"); //(for decryption)
              
            user = db.Tbl_User.FirstOrDefault(x => x.UserId == UserId);

            if (user.UserPassword == oldPassword)
            {
                //user.UserEmail = user.UserEmail;
                user.UserPassword = newPassword;
                user.EditBy = "Admin";
                user.EditDate = DateTime.Now;
                db.SaveChanges();
                var jsonData = new { success = true, message = "User Updated Successfully" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = "User Update Operation Failed !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        [CheckAuthorization]
        public ActionResult User()
        {
          //  var roledata = db.Tbl_UserRole.FirstOrDefault(x => x.RoleId == userSession.RoleId);
            //if ((roledata.RoleName == "Administrator" || roledata.RoleName == "Superadmin" || roledata.RoleName == "Admin") && roledata != null)
            //{
            //    ViewBag.UserOffice = new SelectList(db.HR_Office, "OfficeCode", "OfficeName");
            //}
            //else
            //{
            //    ViewBag.UserOffice = new SelectList(db.HR_Office.Where(x=>x.OfficeCode==userSession.OfficeCode), "OfficeCode", "OfficeName");
            //}
                 ViewBag.UserRole = new SelectList(db.Tbl_UserRole, "RoleId", "RoleName");
            
            return View(db.Tbl_User.Where(x => x.RoleId >= 1).ToList());
        }


        [HttpPost]
        public JsonResult AddUser(Tbl_User user)
        {
            user.UserPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.UserPassword.Trim(), "md5"); //(for encryption)
            //Helpers.SHA1.Encode(user.UserPassword);
            var ExistingUser = (from item in db.Tbl_User where item.UserName == user.UserName && item.UserPassword == user.UserPassword select item).ToList();

            if (ExistingUser.Count > 0)
            {
                var jsonData = new { success = true, message = "This Role Already Assigned !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {

                user.AddBy = "Admin";
                user.AddDate = DateTime.Now;

                db.Tbl_User.Add(user);
                db.SaveChanges();
                var jsonData = new { success = true, message = "User Created Successfully." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserList()
        {
            return PartialView(db.Tbl_User.ToList());
        }

        public ActionResult UpdateUser(Tbl_User user, int UserId, int RoleId,string OfficeCode, string UserName,string UserEmail, string UserPassword, string Status, string Oldpassword)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.UserPassword.Trim(), "md5"); //(for encryption)Helpers.SHA1.Encode(UserPassword);
            string oldpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Oldpassword.Trim(), "md5"); //(for encryption)Helpers.SHA1.Encode(Oldpassword);
            user = db.Tbl_User.FirstOrDefault(x => x.UserId == UserId);

            if (user.UserPassword == oldpassword)
            {
                user.UserEmail = UserEmail;
                user.UserName = UserName;
                user.OfficeCode = OfficeCode;
                user.RoleId = RoleId;
                user.UserPassword = password;
                user.Status = Status;
                user.EditBy = "Admin";
                user.EditDate = DateTime.Now;
                db.SaveChanges();
                var jsonData = new { success = true, message = "User Updated Successfully" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new { success = false, message = "User Update Operation Failed !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteUser(int UserId)
        {
            //var existingdata = (from item in db.Tbl_User where item.RoleId == UserId select item).ToList();

            var data = db.Tbl_User.FirstOrDefault(x => x.UserId == UserId);
            var permission=db.Tbl_UserPermission.Where(x => x.UserId == UserId).ToList();
            db.Tbl_User.Remove(data);
            db.SaveChanges();
            foreach (var item in permission)
            {
                db.Tbl_UserPermission.Remove(item);
                db.SaveChanges();
            }
          
            var jsonData = new { success = true, message = "Deleted Successfully" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckOldPassword(int UserId,string password)
        {
            bool success = false;
  
           string Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"); //(for encryption) Helpers.SHA1.Encode(password);

            var data = db.Tbl_User.FirstOrDefault(x => x.UserId == UserId);
            if (data.UserPassword == Password)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
          
        }

        //---------------------------Menu----------------------------------//

        [CheckAuthorization]
        public ActionResult MenuTab()
        {
            return View(db.Tbl_Menu.ToList());
        }

        [HttpPost]
        public JsonResult AddMenu(Tbl_Menu menu)
        {
            var ExistingMenu = (from item in db.Tbl_Menu where item.MenuName == menu.MenuName select item).ToList();

            if (ExistingMenu.Count > 0)
            {
                var jsonData = new { success = true, message = "This Menu Already Assigned !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                menu.AddBy = "Admin";
                menu.AddDate = DateTime.Now;

                db.Tbl_Menu.Add(menu);
                db.SaveChanges();
                var jsonData = new { success = true, message = "Menu Created Successfully." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MenuList()
        {
            return PartialView(db.Tbl_Menu.ToList());
        }

        public ActionResult UpdateMenu(Tbl_Menu menu, int MenuId, string MenuName, int MenuOrderNo, string Status)
        {
            menu = db.Tbl_Menu.FirstOrDefault(x => x.MenuId == MenuId);
            menu.MenuName = MenuName;
            menu.MenuOrderNo = MenuOrderNo;
            menu.Status = Status;
            menu.EditBy = "Admin";
            menu.EditDate = DateTime.Now;

            db.SaveChanges();
            var jsonData = new { success = true, message = "User Role Updated Successfully" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMenu(int MenuId)
        {
            var existingdata = (from item in db.Tbl_View where item.MenuId == MenuId select item).ToList();

            if (existingdata.Count > 0)
            {
                // show message
                var jsonData = new { success = true, message = "Can not delete the Role. already use in." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = db.Tbl_Menu.FirstOrDefault(x => x.MenuId == MenuId);
                db.Tbl_Menu.Remove(data);
                db.SaveChanges();
                var jsonData = new { success = true, message = "Deleted Successfully" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        
        [CheckAuthorization]
        public ActionResult MenuPage()
        {
            ViewBag.Menu = new SelectList(db.Tbl_Menu, "MenuId", "MenuName");
            return View(db.Tbl_View.ToList());
        }

        [HttpPost]
        public JsonResult AddPage(Tbl_View view)
        {
            var ExistingMenu = (from item in db.Tbl_View where item.PageName == view.PageName select item).ToList();

            if (ExistingMenu.Count > 0)
            {
                var jsonData = new { success = true, message = "This Page Already Assigned !" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                view.PageUrl = "/" + view.Controller + "/" + view.Action;
                view.AddBy = "Admin";
                view.AddDate = DateTime.Now;

                db.Tbl_View.Add(view);
                db.SaveChanges();
                var jsonData = new { success = true, message = "Page Created Successfully." };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewList()
        {
            return PartialView(db.Tbl_View.ToList());
        }

        public ActionResult UpdatePage(Tbl_View view, int ViewId, int MenuId, string PageName, string Controller, string Action, int ViewOrderNo, string Status)
        {
            view = db.Tbl_View.FirstOrDefault(x => x.ViewId == ViewId);
            view.MenuId = MenuId;
            view.PageName = PageName;
            view.Controller = Controller;
            view.Action = Action;
            view.PageUrl = "/" + Controller + "/" + Action;
            view.ViewOrderNo = ViewOrderNo;
            view.Status = Status;
            view.EditBy = "Admin";
            view.EditDate = DateTime.Now;

            db.SaveChanges();
            var jsonData = new { success = true, message = "Page Updated Successfully" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePage(int ViewId)
        {
            //var existingdata = (from item in db.Tbl_View where item.MenuId == MenuId select item).ToList();

            //if (existingdata.Count > 0)
            //{
            //    // show message
            //    var jsonData = new { success = true, message = "Can not delete the Role. already use in." };
            //    return Json(jsonData, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            var data = db.Tbl_View.FirstOrDefault(x => x.ViewId == ViewId);
            db.Tbl_View.Remove(data);
            db.SaveChanges();
            var jsonData = new { success = true, message = "Page Deleted Successfully" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //}
        }

        //---------------------------Menu----------------------------------//
        [CheckAuthorization]
        public ActionResult Permission()
        {
            ViewBag.User = new SelectList(db.Tbl_User.Where(x => x.RoleId > userSession.RoleId), "UserId", "UserName");
            var roledata = db.Tbl_UserRole.FirstOrDefault(x => x.RoleId == userSession.RoleId);
            if (roledata != null)
            {
                if (roledata.RoleName == "Administrator")
                {
                    var existdata = menuList.Where(x => x.UserId == userSession.UserId).ToList();

                   // db.ViewMenuPermission.Where(x => x.UserId == userSession.UserId).ToList();
                    return View(existdata);
                }
                else
                {
                    var existdata = menuList.Where(x => x.UserId == userSession.UserId).ToList();
                    //db.ViewMenuPermission.Where(x => x.UserId == userSession.UserId && x.PermissionStatus == "Active").ToList();
                    return View(existdata);
                }
            }
            return RedirectToAction("Logout", "Security");
        }

        public JsonResult LoadPage(int MenuId, int UserId)
        {
            var existdata = db.Tbl_UserPermission.Where(x => x.UserId == UserId && x.PermissionStatus == "Active").Select(x => new { ViewId = x.ViewId }).ToList();

            var roledata = db.Tbl_UserRole.FirstOrDefault(x => x.RoleId == userSession.RoleId);
            if (roledata != null)
            {
                if (roledata.RoleName == "Administrator")
                {
                    var newdata = db.Tbl_View.Where(x => x.MenuId == MenuId).Select(x => new { ViewId = x.ViewId, PageName = x.PageName }).ToList();
                    var data = new { existdata = existdata, newdata = newdata };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var newdata = menuList.Where(x => x.UserId == userSession.UserId && x.PermissionStatus == "Active" && x.MenuId == MenuId).Select(x => new { ViewId = x.ViewId, PageName = x.PageName }).ToList();
                    //db.ViewMenuPermissions.Where(x => x.UserId == userSession.UserId && x.PermissionStatus == "Active" && x.MenuId == MenuId).Select(x => new { ViewId = x.ViewId, PageName = x.PageName }).ToList();
                    var data = new { existdata = existdata, newdata = newdata };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(JsonRequestBehavior.AllowGet);

        }
        public JsonResult SetViewPermission(Tbl_UserPermission up)
        {
            var existView = db.Tbl_UserPermission.FirstOrDefault(x => x.ViewId == up.ViewId && x.MenuId == up.MenuId && x.UserId == up.UserId);
            if (existView != null)
            {
                existView.PermissionStatus = up.PermissionStatus;
                db.SaveChanges();
            }
            else
            {
                db.Tbl_UserPermission.Add(up);
                db.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

       
    }
}