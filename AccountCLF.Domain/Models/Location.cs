using System;
using System.Collections.Generic;

namespace Model;

public partial class Location
{
    public int Id { get; set; }

    public string? SrNo { get; set; }

    public string? Code { get; set; }

    public string? ShortName { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public int? TypeId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<Location> InverseParent { get; set; } = new List<Location>();

    public virtual Location? Parent { get; set; }

    public virtual ICollection<TransFundBillingDetail> TransFundBillingDetails { get; set; } = new List<TransFundBillingDetail>();

    public virtual MasterTypeDetail? Type { get; set; }
}
