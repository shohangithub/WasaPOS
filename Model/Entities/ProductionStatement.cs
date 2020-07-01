using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
   public class ProductionStatement : BaseEntity
    {
        public long ProductionStatementId { get; set; }
        public DateTime ProductionDate { get; set; }
        
        public int? Swml250Quantity { get; set; }
        
        public int? Sw5mlQuantity { get; set; }
        
        public int? Sw1lQuantity { get; set; }
       
        public int? Sw15lQuintity { get; set; }
       
        public int? Sw2lQuintity { get; set; }
       
        public int? Sw5lQuintity { get; set; }
        
        public int? Sw20lQuintity { get; set; }
       
        public int? Sw20l4Quintity { get; set; }
       
    }
}
