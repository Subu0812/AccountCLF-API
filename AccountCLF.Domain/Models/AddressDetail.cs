using System;
using System.Collections.Generic;

namespace Model;

public partial class AddressDetail
{
    public int Id { get; set; }

    public int? EntityId { get; set; }

    public int? AddressTypeId { get; set; }

    public int? CityId { get; set; }

    public string? PinCode { get; set; }

    public string? Address { get; set; }

    public string? LandMark { get; set; }

    public bool? IsDelete { get; set; }

    public virtual MasterTypeDetail? AddressType { get; set; }

    public virtual Location? City { get; set; }

    public virtual Entity? Entity { get; set; }
}
