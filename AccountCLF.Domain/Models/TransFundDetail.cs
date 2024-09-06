using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundDetail
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? BalanceAmount { get; set; }

    public decimal? Concession { get; set; }

    public int? ConcessionId { get; set; }

    public string? RoundOperator { get; set; }

    public decimal? RoundOff { get; set; }

    public decimal? DiscountPer { get; set; }

    public decimal? Discount { get; set; }

    public DateTime? DueDate { get; set; }

    public virtual MasterTypeDetail? ConcessionNavigation { get; set; }

    public virtual TransFund? FundReference { get; set; }
}
