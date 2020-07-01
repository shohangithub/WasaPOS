using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tbl_User
    {
        public Tbl_User()
        {
            this.Tbl_UserPermission = new HashSet<Tbl_UserPermission>();
        }
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; } 
        public string UserPassword { get; set; }
        public int RoleId { get; set; }
        public string OfficeCode { get; set; }
        public string Status { get; set; }
        public string Adder_IP_Address { get; set; }
        public string AddBy { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public virtual Tbl_UserRole Tbl_UserRole { get; set; }
        public virtual ICollection<Tbl_UserPermission> Tbl_UserPermission { get; set; }
    }
}
