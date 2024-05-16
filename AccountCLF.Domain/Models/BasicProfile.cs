using System;
using System.Collections.Generic;

namespace Model;

public partial class BasicProfile
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Designation { get; set; }

    public virtual MasterTypeDetail? DesignationNavigation { get; set; }

    public virtual Entity? Entity { get; set; }
}
