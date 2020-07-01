using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class SaleDetail :BaseEntity
    {
        public long SaleDetailId { get; set; }

        public long SaleId { get; set; }
        public Sale Sale { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public string QtyCase { get; set; }
        public decimal FacePrice { get; set; }
        public decimal TotalPrice { get; set; }
       
    }
}
