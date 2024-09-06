using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Reciepts
{
    public class CreateRecieptDto
    {
        public int? EntityId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? RecieveAmount { get; set; }
        public decimal? TDSAmount { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? LoanId { get; set; }

        public int? PayMode { get; set; }

        public string? SlipUpload { get; set; }
        public string PayType { get; set; }

        public int? AccountId { get; set; }

        public int? SessionId { get; set; }

        public long? DemoId { get; set; }
        public List<CreateRecieptDeductionsDto>? Deductions { get; set; } = new List<CreateRecieptDeductionsDto>();
        public CreateRecieptTdsDto? Tds { get; set; }

    }
    public class CreateRecieptTdsDto
    {
        public decimal? TdsableAmount { get; set; }

        public decimal? Tdsper { get; set; }

        public decimal? Tds { get; set; }

        public string? PanNo { get; set; }
    }
    public class CreateRecieptDeductionsDto
    {
        public int? EntityId { get; set; }
        public decimal? DeductionAmount { get; set; }

    }
}
