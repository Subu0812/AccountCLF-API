using System;
using System.Collections.Generic;

namespace Model;

public partial class ProfileLink
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public virtual Entity? Entity { get; set; }
}
