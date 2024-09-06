using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundBill
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public string? BillNo { get; set; }

    public DateTime? BillDate { get; set; }

    public virtual TransFund? FundReference { get; set; }
}
