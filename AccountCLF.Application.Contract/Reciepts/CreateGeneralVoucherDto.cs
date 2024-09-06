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
        public int? DRAccount { get; set; }
        public int? CRAccount { get; set; }
        public int? EntityId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Remark { get; set; }

    }
}
