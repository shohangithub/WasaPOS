using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Sale: BaseEntity
    {
        public Sale()
        {
            SaleDetails = new List<SaleDetail>();
        }
        public long SaleId { get; set; }
        
        public string InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal STAmount { get; set; }
        public decimal NetTotal { get; set; }
        public decimal TSSD { get; set; }
        public decimal TSVat { get; set; }
        public decimal TransportCost { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CashReceive { get; set; }
        public decimal? AvgCashReceive { get; set; }
        public decimal DueAmount { get; set; }
        public decimal? AvgDueAmount { get; set; }
        public decimal OtherCost { get; set; }
        
        public ICollection<SaleDetail> SaleDetails { get; set; }

    }
}
