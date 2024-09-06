using System;
using System.Collections.Generic;

namespace Model;

public partial class VoucherType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<TransFund> TransFunds { get; set; } = new List<TransFund>();

    public virtual ICollection<VoucherSrNo> VoucherSrNos { get; set; } = new List<VoucherSrNo>();
}
