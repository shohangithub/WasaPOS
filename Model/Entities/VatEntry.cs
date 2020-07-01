using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class VatEntry: BaseEntity
    {
            [Key]
            public int VatId { get; set; }
            public short VatAmount { get; set; }
    }
}
