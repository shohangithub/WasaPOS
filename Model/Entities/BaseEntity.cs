using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            AddedDate = DateTime.Now;
            IsDeleted = false;

        }
        public Status? Status { get; set; }
        public string AddedById { get; set; }
        //public ApplicationUser AddedBy { get; set; }
        public DateTime AddedDate { get; set; }

        public string ModifiedById { get; set; }
       // public ApplicationUser ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string IpAddress { get; set; }

        public bool IsDeleted { get; set; }
    }
}
