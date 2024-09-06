using System;
using System.Collections.Generic;

namespace Model;

public partial class VoucherSrNo
{
    public int Id { get; set; }

    public int SrNo { get; set; }

    public int? VoucherTypeId { get; set; }

    public int? SessionId { get; set; }

    public int? ClfId { get; set; }

    public virtual Entity? Clf { get; set; }

    public virtual AccountSession? Session { get; set; }

    public virtual ICollection<TransFund> TransFunds { get; set; } = new List<TransFund>();

    public virtual VoucherType? VoucherType { get; set; }
}
