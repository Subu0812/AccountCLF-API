using System;
using System.Collections.Generic;

namespace Model;

public partial class EntityType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();
}
