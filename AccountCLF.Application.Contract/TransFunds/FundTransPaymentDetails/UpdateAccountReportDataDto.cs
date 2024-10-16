using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.FundTransPaymentDetails
{
    public class UpdateAccountReportDataDto
    {
        public decimal? Amount { get; set; }
        public string? TransType { get; set; }
        public int? PaymentModeId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? PayDate { get; set; }

    }
}
