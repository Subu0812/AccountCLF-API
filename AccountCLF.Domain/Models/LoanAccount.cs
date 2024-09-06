using System;
using System.Collections.Generic;

namespace Model;

public partial class LoanAccount
{
    public int Id { get; set; }

    public string? SrNo { get; set; }

    public decimal? LoanAmount { get; set; }

    public decimal? PayableAmount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? Status { get; set; }

    public int? EntityId { get; set; }

    public int? FundReferenceId { get; set; }

    public int? LoantenureId { get; set; }

    public virtual Entity? Entity { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual ICollection<LoanAccountDetail> LoanAccountDetails { get; set; } = new List<LoanAccountDetail>();

    public virtual LoanTenure? Loantenure { get; set; }
}
