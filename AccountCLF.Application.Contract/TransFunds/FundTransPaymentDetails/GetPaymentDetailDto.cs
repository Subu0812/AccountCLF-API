using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.FundTransPaymentDetails
{
    public class GetPaymentDetailDto
    {
        public int Id { get; set; }

        public int? FundReferenceId { get; set; }

        public string? ReciptNo { get; set; }

        public long? MaxNo { get; set; }

        public decimal? TransactionAmount { get; set; }

        public string? TransactionStatus { get; set; }

        public string? TransactionId { get; set; }

        public string? BankReferenceId { get; set; }

        public decimal? BankReferenceAmount { get; set; }

        public string? BankReferenceStatus { get; set; }

        public int? LedgerId { get; set; }

        public int? PaymentModeId { get; set; }

        public int? BankId { get; set; }

        public string? UploadedDoc { get; set; }

        public int? DaybookId { get; set; }

        public int? AppliedAmountId { get; set; }

        public DateTime? ApplyDate { get; set; }

        public DateTime? PayDate { get; set; }
    }
}
