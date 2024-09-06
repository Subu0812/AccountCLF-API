using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundLink
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public decimal? Amount { get; set; }

    public int? ParentId { get; set; }

    public int? LinkFundReferenceId { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual ICollection<TransFundLink> InverseParent { get; set; } = new List<TransFundLink>();

    public virtual TransFundLink? Parent { get; set; }
}
