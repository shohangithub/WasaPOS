using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
   public class DailySalesStatement : BaseEntity
    {
        public long DailySalesStatementId { get; set; }
        public string ReceieptNo { get; set; }
        public DateTime DailyStatementDate { get; set; }
        public string CustomerName { get; set; }

        public int? Swml250Quantity { get; set; }
        public decimal? Swml250TotalPrice { get; set; }


        public int? Sw5mlQuantity { get; set; }
        public decimal? Sw5mlTotalPrice { get; set; }


        public int? Sw1lQuantity { get; set; }
        public decimal? Sw1lTotalPrice { get; set; }


        public int? Sw15lQuintity { get; set; }
        public decimal Sw15lTotalPrice { get; set; }


        public int? Sw2lQuintity { get; set; }
        public decimal? Sw2lTotalPrice { get; set; }


        public int? Sw5lQuintity { get; set; }
        public decimal? Sw5lTotalPrice { get; set; }


        public int? Sw20lQuintity { get; set; }
        public decimal? Sw20lTotalPrice { get; set; }


        public int? Sw20l4Quintity { get; set; }
        public decimal? Sw20l4TotalPrice { get; set; }
    }
}
