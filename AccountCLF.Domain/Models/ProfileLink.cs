using System;
using System.Collections.Generic;

namespace Model;

public partial class ProfileLink
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? FatherName { get; set; }

    public int? MotherName { get; set; }

    public virtual Entity? Entity { get; set; }

    public virtual Entity? FatherNameNavigation { get; set; }

    public virtual Entity? MotherNameNavigation { get; set; }
}
