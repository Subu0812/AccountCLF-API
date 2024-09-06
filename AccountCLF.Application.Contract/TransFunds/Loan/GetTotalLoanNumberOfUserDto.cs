using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class GetTotalLoanNumberOfUserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LoanNo { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? InterestAmount { get; set; }    
        public decimal? PayableAmount { get; set; }
        public string? LoanTenure { get; set; }
        public decimal? InterestRate { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool Status { get; set; }

    }
}
