using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundPaymentDetail
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

    public int? TransType { get; set; }

    public virtual MasterTypeDetail? Bank { get; set; }

    public virtual Daybook? Daybook { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual Entity? Ledger { get; set; }

    public virtual MasterTypeDetail? PaymentMode { get; set; }

    public virtual MasterTypeDetail? TransTypeNavigation { get; set; }
}
