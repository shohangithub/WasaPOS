using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class CompanySetup : BaseEntity
    {
        public int CompanySetupId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public byte[] Image { get; set; }
    }
}
