using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Loan
{
    public class GetLoanDto
    {
        public int Id { get; set; }

        public string? SrNo { get; set; }

        public int? EmiMonth { get; set; }

        public decimal? PayableAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
