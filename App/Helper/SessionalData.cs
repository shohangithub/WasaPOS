using Data;
using Model;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Security;

namespace App.Helper
{
 
    public class SessionalData
    {
      
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool CheckUser(string userName, string Password)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Trim(), "md5"); //(for encryption) Helpers.SHA1.Encode(Password);
            var user = db.Tbl_User.FirstOrDefault(x => x.UserEmail == userName && x.UserPassword == password && x.Status == "Active");
          

            if (user != null)
            {
                HttpContext.Current.Session["UserSession"] = user;

                //List<ViewMenuPermission> menuList = db.ViewMenuPermissions.Where(x => x.MenuStatus == "Active" && x.UserId==user.UserId && x.PermissionStatus=="Active").ToList();
                //if (menuList.Count > 0)
                //{
                //    HttpContext.Current.Session["MenuList"] = menuList;
                //}
                 List<ViewMenuPermission> menuList = db.Tbl_UserPermission.Include(x=>x.Tbl_User).Include(x => x.Tbl_View).Where(x => x.UserId == user.UserId && x.PermissionStatus == "Active")
                    .Select(item=>new ViewMenuPermission {
                        Action = item.Tbl_View.Action,
                        Controller = item.Tbl_View.Controller,
                        MenuId = item.MenuId,
                        MenuName = item.Tbl_View.Tbl_Menu.MenuName,
                        MenuOrderNo = item.Tbl_View.Tbl_Menu.MenuOrderNo,
                        MenuStatus = item.Tbl_View.Tbl_Menu.Status,
                        OfficeCode = "",
                        PageName = item.Tbl_View.PageName,
                        PageUrl = item.Tbl_View.PageUrl,
                        PermissionStatus = item.PermissionStatus,
                        RoleName = item.Tbl_User.Tbl_UserRole.RoleName,
                        Status = item.Tbl_User.Status,
                        UserId = item.UserId,
                        UserName = item.Tbl_User.UserName,
                        UserEmail = item.Tbl_User.UserEmail,
                        UserPassword = item.Tbl_User.UserPassword,
                        ViewId = item.ViewId,
                        ViewOrderNo = item.Tbl_View.ViewOrderNo
                    }).ToList();

                //List<ViewMenuPermission> menuList = new List<ViewMenuPermission>();
                //var permissionData = db.Tbl_UserPermission.Include(x => x.Tbl_User).Include(x => x.Tbl_View).Where(x => x.UserId == user.UserId && x.PermissionStatus == "Active").ToList();

                //foreach (var item in permissionData)
                //{
                //    menuList.Add(new ViewMenuPermission
                //    {
                //        Action = item.Tbl_View.Action,
                //        Controller = item.Tbl_View.Controller,
                //        MenuId = item.MenuId,
                //        MenuName = item.Tbl_View.Tbl_Menu.MenuName,
                //        MenuOrderNo = item.Tbl_View.Tbl_Menu.MenuOrderNo,
                //        MenuStatus = item.Tbl_View.Tbl_Menu.Status,
                //        OfficeCode = "",
                //        PageName = item.Tbl_View.PageName,
                //        PageUrl=item.Tbl_View.PageUrl,
                //        PermissionStatus=item.PermissionStatus,
                //        RoleName=item.Tbl_User.Tbl_UserRole.RoleName,
                //        Status=item.Tbl_User.Status,
                //        UserId=item.UserId,
                //        UserName=item.Tbl_User.UserName,
                //        UserPassword=item.Tbl_User.UserPassword,
                //        ViewId=item.ViewId,
                //        ViewOrderNo=item.Tbl_View.ViewOrderNo
                //    });
                //}

                //List<ViewMenuPermission> menuList = db.ViewMenuPermission.Where(x => x.Tbl_View.Tbl_Menu.Status == "Active" && x.Tbl_User.UserId == user.UserId && x.Tbl_UserPermission.PermissionStatus == "Active").ToList();
                if (menuList.Count > 0)
                {
                    HttpContext.Current.Session["MenuList"] = menuList;
                }

                return true;
            }
            else
            {
                HttpContext.Current.Session["UserSession"] = null;
                HttpContext.Current.Session["MenuList"] = null;
                return false;
            }
        }
      
    }
}