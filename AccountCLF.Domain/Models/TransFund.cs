using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFund
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

    public bool? Status { get; set; }

    public DateTime? PaymentConfirmDate { get; set; }

    public string? SlipUpload { get; set; }

    public string? Remark { get; set; }

    public virtual ICollection<Daybook> Daybooks { get; set; } = new List<Daybook>();

    public virtual Entity? Entity { get; set; }

    public virtual MasterTypeDetail? Franchise { get; set; }

    public virtual MasterTypeDetail? LedgerHead { get; set; }

    public virtual ICollection<LoanAccountDetail> LoanAccountDetails { get; set; } = new List<LoanAccountDetail>();

    public virtual ICollection<LoanAccount> LoanAccounts { get; set; } = new List<LoanAccount>();

    public virtual MasterTypeDetail? PayModeNavigation { get; set; }

    public virtual AccountSession? Session { get; set; }

    public virtual Entity? Staff { get; set; }

    public virtual ICollection<TransFundBillingDetail> TransFundBillingDetails { get; set; } = new List<TransFundBillingDetail>();

    public virtual ICollection<TransFundBill> TransFundBills { get; set; } = new List<TransFundBill>();

    public virtual ICollection<TransFundDetail> TransFundDetails { get; set; } = new List<TransFundDetail>();

    public virtual ICollection<TransFundLink> TransFundLinks { get; set; } = new List<TransFundLink>();

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetails { get; set; } = new List<TransFundPaymentDetail>();

    public virtual ICollection<TransFundRemark> TransFundRemarks { get; set; } = new List<TransFundRemark>();

    public virtual ICollection<TransFundTd> TransFundTds { get; set; } = new List<TransFundTd>();

    public virtual VoucherSrNo? VoucherNoNavigation { get; set; }

    public virtual VoucherType? VoucherTypeNavigation { get; set; }
}
