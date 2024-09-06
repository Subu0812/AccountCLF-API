using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Advances
{
    public class CreateAdvancedDto
    {
        public int FundReferenceId { get; set; }

        public string Remarks { get; set; }
    }
}
