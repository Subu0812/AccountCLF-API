using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class ProfileLink
{
    [Key]

    public int Id { get; set; }

    public int? EntityId { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public  Entity? Entity { get; set; }

}
