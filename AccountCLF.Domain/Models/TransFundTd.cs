using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundTd
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public decimal? TdsableAmount { get; set; }

    public decimal? Tdsper { get; set; }

    public decimal? Tds { get; set; }

    public string? PanNo { get; set; }

    public int? SectionId { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual MasterTypeDetail? Section { get; set; }
}
