using System;
using System.Collections.Generic;

namespace Model;

public partial class LoanTenure
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<LoanAccount> LoanAccounts { get; set; } = new List<LoanAccount>();
}
