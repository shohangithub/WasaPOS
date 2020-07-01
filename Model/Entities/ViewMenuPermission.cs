using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ViewMenuPermission
    {
        public string PageName { get; set; }
        public int ViewId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int MenuOrderNo { get; set; }
        public int ViewOrderNo { get; set; }
        public int UserId { get; set; }
        public string PermissionStatus { get; set; }
        public string UserPassword { get; set; }
        public string MenuStatus { get; set; }
        public string PageUrl { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string OfficeCode { get; set; }

    }
}
