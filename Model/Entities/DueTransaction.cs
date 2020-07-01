using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
   public class DueTransaction: BaseEntity
    {
        public long DueTransactionId { get; set; }
        public string DueTR_Id { get; set; }
        public string TrType { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string TrDescription { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
    }
}
