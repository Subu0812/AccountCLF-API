using System;
using System.Collections.Generic;

namespace Model;

public partial class LoanInterest
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }

    public bool? IsActive { get; set; }
}
