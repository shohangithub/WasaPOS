using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class SaleViewModels
    {
        public SaleViewModels()
        {
            SaleDetails = new List<SaleDetail>();
            SalesStatements = new List<SalesStatement>();

        }
        public long SaleId { get; set; }
        
        public string InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal STAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TSSD { get; set; }
        public decimal TSVat { get; set; }
        public decimal TransportCost { get; set; }
        public decimal NetTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CashReceive { get; set; }
        public decimal DueAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public decimal OtherCost { get; set; }

        public List<SaleDetail> SaleDetails { get; set; }

        public long Swml250SaleDetailId{ get; set; }
        public int Swml250CurrentStock { get; set; }
        
        public int Swml250Quintity { get; set; }
        public string Swml250QtyCase { get; set; }
        public decimal Swml250FacePrice { get; set; }
        public decimal Swml250TotalPrice { get; set; }

        public long Sw5mlSaleDetailId { get; set; }
        public int Sw5mlCurrentStock { get; set; }
        
        public int Sw5mlQuintity { get; set; }
        public string Sw5mlQtyCase { get; set; }
        public decimal Sw5mlFacePrice { get; set; }
        public decimal Sw5mlTotalPrice { get; set; }

        public long Sw1lSaleDetailId { get; set; }
        public int Sw1lCurrentStock { get; set; }
       
        public int Sw1lQuintity { get; set; }
        public string Sw1lQtyCase { get; set; }
        public decimal Sw1lFacePrice { get; set; }
        public decimal Sw1lTotalPrice { get; set; }

        public long Sw15lSaleDetailId { get; set; }
        public int Sw15lCurrentStock { get; set; }
       
        public int Sw15lQuintity { get; set; }
        public string Sw15lQtyCase { get; set; }
        public decimal Sw15lFacePrice { get; set; }
        public decimal Sw15lTotalPrice { get; set; }

        public long Sw2lSaleDetailId { get; set; }
        public int Sw2lCurrentStock { get; set; }
       
        public int Sw2lQuintity { get; set; }
        public string Sw2lQtyCase { get; set; }
        public decimal Sw2lFacePrice { get; set; }
        public decimal Sw2lTotalPrice { get; set; }

        public long Sw5lSaleDetailId { get; set; }
        public int Sw5lCurrentStock { get; set; }
        
        public int Sw5lQuintity { get; set; }
        public string Sw5lQtyCase { get; set; }
        public decimal Sw5lFacePrice { get; set; }
        public decimal Sw5lTotalPrice { get; set; }

        public long Sw20lSaleDetailId { get; set; }
        public int Sw20lCurrentStock { get; set; }
        
        public int Sw20lQuintity { get; set; }
        public string Sw20lQtyCase { get; set; }
        public decimal Sw20lFacePrice { get; set; }
        public decimal Sw20lTotalPrice { get; set; }

        public long Sw20l4SaleDetailId { get; set; }
        public int Sw20l4CurrentStock { get; set; }
       
        public int Sw20l4Quintity { get; set; }
        public string Sw20l4QtyCase { get; set; }
        public decimal Sw20l4FacePrice { get; set; }
        public decimal Sw20l4TotalPrice { get; set; }

        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }

        public string Phone { get; set; }

        public List<SalesStatement> SalesStatements { get; set; }
        public long SalesStatementId { get; set; }

    }
}
