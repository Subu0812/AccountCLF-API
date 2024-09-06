using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class LoanCalculationDto
    {
        public int EmiMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal TotalInterestAmount { get; set; }
        public decimal EMIAmountMonth { get; set; }
    }
}
