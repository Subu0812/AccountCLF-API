using System;
using System.Collections.Generic;

namespace Model;

public partial class VoucherType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<VoucherSrNo> VoucherSrNos { get; set; } = new List<VoucherSrNo>();
}
