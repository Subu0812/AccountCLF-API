using AccountCLF.Application.Contract.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.FundTransPaymentDetails
{
    public class CreatePaymentDetailsDto
    {
        public string? BankReferenceId { get; set; }

        public int? PaymentModeId { get; set; }

        public int? BankId { get; set; }

        public int? EntityBankAccountTypeId { get; set; }

        //public IFormFile? UploadedDoc { get; set; }

        public int? AppliedAmountId { get; set; }

        public int? SessionId { get; set; }

        public long? DemoId { get; set; }

        public int? EntityId { get; set; }

        public decimal? Amount { get; set; }
        public decimal? RecieveAmount { get; set; }
        public decimal? TDSAmount { get; set; }
        public decimal? BankChargeAmount { get; set; }
        public DateTime? EntryDate { get; set; }

        public int? PayMode { get; set; }

        public int? AccountId { get; set; }

        public string? SlipUpload { get; set; }
        public List<CreateDeductionsDto>? Deductions { get; set; } = new List<CreateDeductionsDto>();
        public CreateTdsDto? Tds { get; set; }

    }
    public class CreateTdsDto
    {
        public decimal? TdsableAmount { get; set; }

        public decimal? Tdsper { get; set; }

        public decimal? Tds { get; set; }

        public string? PanNo { get; set; }
    }
    public class CreateDeductionsDto
    {
        public int? EntityId { get; set; }
        public decimal? DeductionAmount { get; set; }

    }
}
