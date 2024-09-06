using System;
using System.Collections.Generic;

namespace Model;

public partial class LoanAccountDetail
{
    public int Id { get; set; }

    public int? LoanAccountId { get; set; }

    public int? EmiMonth { get; set; }

    public decimal? LoanAmount { get; set; }

    public decimal? PayableAmount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? Status { get; set; }

    public int? EntityId { get; set; }

    public int? FundReferenceId { get; set; }

    public decimal? InterestAmount { get; set; }

    public decimal? InterestPercentage { get; set; }

    public virtual Entity? Entity { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual LoanAccount? LoanAccount { get; set; }
}
