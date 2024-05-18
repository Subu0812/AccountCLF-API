using System;
using System.Collections.Generic;

namespace Model;

public partial class AccountGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public int? TypeId { get; set; }

    public string? InOut { get; set; }

    public string? Placcount { get; set; }

    public string? NatureType { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual ICollection<AccountGroup> InverseParent { get; set; } = new List<AccountGroup>();

    public virtual AccountGroup? Parent { get; set; }
}
