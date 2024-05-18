using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class MasterType
{
    [Key]

    public int Id { get; set; }

    public decimal? SrNo { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public DateTime? Date { get; set; }

    public int? IsDelete { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<MasterType> InverseParent { get; set; } = new List<MasterType>();

    public virtual ICollection<MasterTypeDetail> MasterTypeDetails { get; set; } = new List<MasterTypeDetail>();

    public virtual MasterType? Parent { get; set; }
}
