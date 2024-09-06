using System;
using System.Collections.Generic;

namespace Model;

public partial class TransFundBillingDetail
{
    public int Id { get; set; }

    public int? FundReferenceId { get; set; }

    public string? ReceiptName { get; set; }

    public string? ReceiptCoName { get; set; }

    public int? LocationId { get; set; }

    public string? PinCode { get; set; }

    public string? Address { get; set; }

    public string? LandMark { get; set; }

    public string? ContactDetail { get; set; }

    public string? WebsiteUrl { get; set; }

    public int? AddressTypeId { get; set; }

    public virtual MasterTypeDetail? AddressType { get; set; }

    public virtual TransFund? FundReference { get; set; }

    public virtual Location? Location { get; set; }
}
