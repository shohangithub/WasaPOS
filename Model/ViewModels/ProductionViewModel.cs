using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class ProductionViewModel
    {
        public string ProductionShift { get; set; }
        public DateTime ProductionDate { get; set; }
        public List<ProductionDetail> ProductionDetails { get; set; }
    }
}
