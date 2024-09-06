using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class LoanAccountDto
    {
        public int EntityId { get; set; }
        public decimal LoanAmount { get; set; }
        public int EmiMonth { get; set; }
        public int FundReferenceId { get; set; }
        public decimal InterestPercentage { get; set; }
        public int LoantenureId { get; set; }



    }
}
