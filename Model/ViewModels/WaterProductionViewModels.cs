using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
   public class WaterProductionViewModels
    {
        public string Size { get; set; }
        public DateTime ProductionDate { get; set; }
        public int Quantity { get; set; }
        public string QtyCase { get; set; }
        public decimal Litter { get; set; }
    }
}
