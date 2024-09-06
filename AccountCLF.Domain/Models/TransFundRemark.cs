using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundRemark
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public string? Remarks { get; set; }

    public virtual TransFund? FundReference { get; set; }
}
