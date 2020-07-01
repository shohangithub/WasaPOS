using Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Helper
{
    public static class Menu
    {
        public static MvcHtmlString MenuList(this HtmlHelper helper, List<ViewMenuPermission> menuobject)
        {
            if (menuobject == null || menuobject.Count == 0)
                return MvcHtmlString.Empty;
            //return MvcHtmlString.Create(buildMenuItems(menuobject));
            return MvcHtmlString.Empty;
        }
        //private static string buildMenuItems(List<ViewMenuPermission> menuobject)
        //{
        //   var menuList= menuobject.Select(x => new { x.MenuId, x.MenuName, x.MenuOrderNo }).ToList().Distinct().OrderBy(x=>x.MenuOrderNo);
      
        //  var GrandParentUL = new TagBuilder("ul");
        //  var ListTag = new TagBuilder("li");
        //    ListTag.AddCssClass("products");
        //  var DivTag = new TagBuilder("div");
        //    DivTag.GenerateId("cssmenu");
        //  var parentTag = new TagBuilder("ul");
        //  foreach (var menu in menuList)
        //   {

             
              
        //      //----------------------span tag-------------------
        //      var spanTag = new TagBuilder("span");
        //      spanTag.SetInnerText(menu.MenuName);

        //      //----------------------Anchor tag-------------------
        //       var anchorTag = new TagBuilder("a");
        //       anchorTag.MergeAttribute("href", "#");
        //       anchorTag.InnerHtml = spanTag.ToString();

        //       //----------------------Anchor tag-------------------
        //       var menuTag = new TagBuilder("li");
        //       menuTag.AddCssClass("has-sub");
        //       menuTag.AddCssClass("active");

        //      var PageList= menuobject.Where(x=>x.MenuId == menu.MenuId).Select(x => new { x.MenuId, x.PageName,x.PageUrl,x.ViewOrderNo }).ToList().OrderBy(x=>x.ViewOrderNo);
        //      var submenuTag = new TagBuilder("ul");
        //      foreach (var subitem in PageList)
        //       {

        //           //----------------------Anchor tag-------------------
        //           var pageanchorTag = new TagBuilder("a");
        //           pageanchorTag.MergeAttribute("href", subitem.PageUrl.ToString());
        //           pageanchorTag.SetInnerText(subitem.PageName);

        //           var pageliTag = new TagBuilder("li");
        //           pageliTag.InnerHtml = pageanchorTag.ToString();
        //           submenuTag.InnerHtml += pageliTag;

        //       }
        //      menuTag.InnerHtml = anchorTag.ToString() + submenuTag;
        //      parentTag.InnerHtml += menuTag;
        //      DivTag.InnerHtml = parentTag.ToString();
        //      ListTag.InnerHtml = DivTag.ToString();
        //   }
        //  GrandParentUL = ListTag;

        //  return GrandParentUL.ToString();
        //}

    }
}