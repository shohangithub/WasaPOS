using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tbl_View
    {
        public Tbl_View()
        {
            this.Tbl_UserPermission = new HashSet<Tbl_UserPermission>();
        }
        [Key]
        public int ViewId { get; set; }
        public int MenuId { get; set; }
        public string PageName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string PageUrl { get; set; }
        public int ViewOrderNo { get; set; }
        public string Status { get; set; }
        public string AddBy { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }

        public virtual Tbl_Menu Tbl_Menu { get; set; }
        public virtual ICollection<Tbl_UserPermission> Tbl_UserPermission { get; set; }
    }
}
