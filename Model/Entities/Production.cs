using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Production : BaseEntity
    {
        public Production()
        {
            ProductionDetails = new List<ProductionDetail>();
        }

        public long ProductionId { get; set; }
        public string ProductionShift { get; set; }
        public DateTime ProductionDate { get; set; }

        public ICollection<ProductionDetail> ProductionDetails { get; set; }
    }
}
