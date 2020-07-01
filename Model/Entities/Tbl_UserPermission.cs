using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tbl_UserPermission
    {
        [Key]
        public int PermissionId { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int ViewId { get; set; }
        public string PermissionStatus { get; set; }

        public virtual Tbl_User Tbl_User { get; set; }
        public virtual Tbl_View Tbl_View { get; set; }
    }
}
