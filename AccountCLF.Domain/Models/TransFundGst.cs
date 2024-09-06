using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundGst
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public decimal? Igst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Cgst { get; set; }

    public decimal? TaxPer { get; set; }

    public string? Gstno { get; set; }

    public virtual TransFund? FundReference { get; set; }
}
