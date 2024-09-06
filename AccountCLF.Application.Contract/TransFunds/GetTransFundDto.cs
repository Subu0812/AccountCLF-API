using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds
{
    public class GetTransFundDto
    {
        public int Id { get; set; }

        public int? EntityId { get; set; }

        public int? VoucherNo { get; set; }

        public int? VoucherType { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? Date { get; set; }

        public int? SessionId { get; set; }

        public int? FranchiseId { get; set; }

        public int? StaffId { get; set; }

        public decimal? TaxableAmount { get; set; }

        public int? PayMode { get; set; }

        public int? LedgerHeadId { get; set; }

        public int? Status { get; set; }

        public DateTime? PaymentConfirmDate { get; set; }

        public string? SlipUpload { get; set; }
    }
}
