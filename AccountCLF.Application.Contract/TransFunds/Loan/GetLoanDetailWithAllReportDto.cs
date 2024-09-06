using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class GetLoanDetailWithAllReportDto
    {
        public string? Name { get; set; }
        public int EntityId { get; set; }
        public decimal? PendingAmount { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public bool? Status { get; set; }


    }
}
