using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Charges_Expenses
{
    public class GetUserTDSDetailsDto
    {
        public DateTime? Date { get; set; }
        public string ParticularName { get; set; }
        public string PanNumber { get; set; }
        public string Section { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TdsPayable { get; set; }
    }
}
