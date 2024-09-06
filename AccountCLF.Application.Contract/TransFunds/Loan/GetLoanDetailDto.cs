using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class GetLoanDetailDto
    {
        public int Id { get; set; }
        public int? EmiMonth { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? InterestAmount { get; set; }
        public decimal? PayableAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Status { get; set; }

    }
}
