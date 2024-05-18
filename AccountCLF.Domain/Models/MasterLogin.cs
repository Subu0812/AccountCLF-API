using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class MasterLogin
{
    [Key]

    public int Id { get; set; }

    public int? EntityId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int? Status { get; set; }

    public virtual Entity? Entity { get; set; }
}
