using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
   public class Product : BaseEntity
    {
        public Product()
        {
            ProductionDetails = new List<ProductionDetail>();
            SaleDetails= new List<SaleDetail>();
            SalesStatements = new List<SalesStatement>();
        }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductShortName { get; set; }
        public string Size { get; set; }
        public decimal SizeInML { get; set; }
        public string Uint { get; set; }
        public decimal CostPrice { get; set; }
        public decimal FacePrice { get; set; }
        public decimal VatInParcent { get; set; }
        public decimal SDInParcent { get; set; }
        public int CurrentStoke { get; set; }
        public string ProductCategory { get; set; }

        public ICollection<ProductionDetail> ProductionDetails { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; }
        public ICollection<SalesStatement> SalesStatements { get; set; }
    }
}
