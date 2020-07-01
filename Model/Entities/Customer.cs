using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            Sales = new List<Sale>();
            SalesStatements = new List<SalesStatement>();
        }
        public long CustomerId { get; set; }
        public string CustomerUId{ get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal TotalDueAmount { get; set; }
        public CustomerType CustomerType { get; set; }
        public ICollection<DueTransaction> DueTransactions { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public ICollection<SalesStatement> SalesStatements { get; set; }
    }
}
