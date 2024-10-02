using System;
using System.Collections.Generic;

namespace Model;

public partial class BankDetail
{
    public int Id { get; set; }

    public int EntityId { get; set; }

    public int? SrNo { get; set; }

    public string BeneficiaryName { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string Ifsccode { get; set; } = null!;

    public int? ParentId { get; set; }

    public int BankId { get; set; }

    public bool IsActive { get; set; }

    public virtual MasterTypeDetail Bank { get; set; } = null!;

    public virtual Entity Entity { get; set; } = null!;

    public virtual ICollection<BankDetail> InverseParent { get; set; } = new List<BankDetail>();

    public virtual BankDetail? Parent { get; set; }
}
