using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Reciepts
{
    public class CreateGeneralVoucherDto
    {
        public int? SessionId { get; set; }
        public DateTime? EntryDate { get; set; }
        public int EntityId { get; set; }
        public string TransType { get; set; }
        public decimal? Amount { get; set; }

        public decimal? TotalAmount { get; set; }
        public string? Remark { get; set; }
        public IEnumerable<CreateLedgerDto>? Ledger { get; set; }

    }
    public class CreateLedgerDto
    {
        public decimal? Amount { get; set; }
        public int? LedgerId { get; set; }
    }
}
