using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class ContactProfile
{
    [Key]

    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? ContactTypeId { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

    public virtual MasterTypeDetail? ContactType { get; set; }

    public virtual Entity? Entity { get; set; }
}
