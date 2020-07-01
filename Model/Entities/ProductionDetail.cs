using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ProductionDetail : BaseEntity
    {
        public long ProductionDetailId { get; set; }

        public Production Production { get; set; }
        public long ProductionId { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string QtyCase { get; set; }
        public decimal Litter { get; set; }


    }
}
