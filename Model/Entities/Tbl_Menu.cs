using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tbl_Menu
    {
        public Tbl_Menu()
        {
            this.Tbl_View = new HashSet<Tbl_View>();
        }
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int MenuOrderNo { get; set; }
        public string Status { get; set; }
        public string AddBy { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public virtual ICollection<Tbl_View> Tbl_View { get; set; }
    }
}
