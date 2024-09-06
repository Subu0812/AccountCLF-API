using System;
using System.Collections.Generic;

namespace Model;

public partial class Otp
{
    public int Id { get; set; }

    public int Otp1 { get; set; }

    public DateTime ExpirationTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsChecked { get; set; }

    public string? ForOtp { get; set; }

    public string? NewMobile { get; set; }

    public int EntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}
