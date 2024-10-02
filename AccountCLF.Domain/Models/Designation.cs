using System;
using System.Collections.Generic;

namespace Model;

public partial class Designation
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? DesignationId { get; set; }

    public int? ReferenceId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<BasicProfile> BasicProfiles { get; set; } = new List<BasicProfile>();

    public virtual MasterTypeDetail? DesignationNavigation { get; set; }

    public virtual Entity? Entity { get; set; }

    public virtual Entity? Reference { get; set; }
}
