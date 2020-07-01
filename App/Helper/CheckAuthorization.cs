using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Model.Entities;

namespace App.Helper
{
    public partial class CheckAuthorization : AuthorizeAttribute
    {
        public Tbl_User userSession = null;
        public static List<ViewMenuPermission> menuList = null;
      
       public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;


            userSession = HttpContext.Current.Session["UserSession"] as Tbl_User;
            menuList = HttpContext.Current.Session["MenuList"] as List<ViewMenuPermission>;
           
            if (userSession != null)
            {
                if (!string.IsNullOrEmpty(userSession.UserName) && !string.IsNullOrEmpty(userSession.UserPassword) && userSession.Status =="Active")
                {
                    //var View = menuList.FirstOrDefault(x => x.Action == actionName && x.Controller == controllerName && x.UserId == userSession.UserId && x.PermissionStatus == "Active");
                    var View = menuList.FirstOrDefault(x => x.Action == actionName && x.Controller == controllerName && x.UserId == userSession.UserId && x.PermissionStatus == "Active");

                    if (View == null)
                    {
                        //if (actionName != "Index" && controllerName != "Home")
                        //{
                        //    ClearSession(filterContext);
                        //}
                        
                        ReturnDashboard(filterContext);
                    }
                }
            }
            else
            {
                ClearSession(filterContext);
            }
        }
        private static void ClearSession(AuthorizationContext filterContext)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Security", action = "Login"}));
        }

        private static void ReturnDashboard(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }
    }
}
