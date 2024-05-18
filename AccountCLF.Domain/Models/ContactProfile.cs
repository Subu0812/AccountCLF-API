using System;
using System.Collections.Generic;

namespace Model;

public partial class ContactProfile
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? ContactTypeId { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

    public virtual MasterTypeDetail? ContactType { get; set; }

    public virtual Entity? Entity { get; set; }
}
