using System;
using System.Collections.Generic;

namespace Model;

public partial class BankLink
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? BankId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Entity? Bank { get; set; }

    public virtual Entity? Entity { get; set; }
}
