using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tbl_UserRole
    {
        public Tbl_UserRole()
        {
            this.Tbl_User = new HashSet<Tbl_User>();
        }
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string AddBy { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public virtual ICollection<Tbl_User> Tbl_User { get; set; }
    }
}
