using System;
using System.Collections.Generic;

namespace Model;

public partial class AccountSession
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? IsShow { get; set; }

    public virtual ICollection<Daybook> Daybooks { get; set; } = new List<Daybook>();

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual ICollection<TransFund> TransFunds { get; set; } = new List<TransFund>();

    public virtual ICollection<VoucherSrNo> VoucherSrNos { get; set; } = new List<VoucherSrNo>();
}
