using System;
using System.Collections.Generic;

namespace Model;

public partial class DocumentProfile
{
    public int Id { get; set; }

    public decimal? SrNo { get; set; }

    public int? EntityId { get; set; }

    public int? DocType { get; set; }

    public string? Path { get; set; }

    public DateTime? InsDate { get; set; }

    public int? DocExtensionId { get; set; }

    public string? AltTag { get; set; }

    public string? Description { get; set; }

    public int? IsActive { get; set; }

    public string? Name { get; set; }

    public bool? IsDelete { get; set; }

    public virtual MasterTypeDetail? DocExtension { get; set; }

    public virtual MasterTypeDetail? DocTypeNavigation { get; set; }

    public virtual Entity? Entity { get; set; }
}
