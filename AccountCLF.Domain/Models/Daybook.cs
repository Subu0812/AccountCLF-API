using System;
using System.Collections.Generic;

namespace Model;

public partial class Daybook
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public int? AccountId { get; set; }

    public decimal? Amount { get; set; }

    public string? TransType { get; set; }

    public int? FranchiseId { get; set; }

    public int? StaffId { get; set; }

    public int? SessionId { get; set; }

    public bool? Status { get; set; }

    public long? DemoId { get; set; }

    public int? ParentId { get; set; }

    public virtual Entity? Account { get; set; }

    public virtual Entity? Franchise { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual ICollection<Daybook> InverseParent { get; set; } = new List<Daybook>();

    public virtual Daybook? Parent { get; set; }

    public virtual AccountSession? Session { get; set; }

    public virtual Entity? Staff { get; set; }

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetails { get; set; } = new List<TransFundPaymentDetail>();
}
