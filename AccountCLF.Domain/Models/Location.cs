using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class Location
{
    [Key]

    public int Id { get; set; }

    public string? SrNo { get; set; }

    public string? Code { get; set; }

    public string? ShortName { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public int? TypeId { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<Location> InverseParent { get; set; } = new List<Location>();

    public virtual Location? Parent { get; set; }

    public virtual MasterTypeDetail? Type { get; set; }
}
