using System;
using System.Collections.Generic;

namespace Model;

public partial class MasterTypeDetail
{
    public int Id { get; set; }

    public decimal? SrNo { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public int? TypeId { get; set; }

    public int? IsActive { get; set; }

    public int? IsDelete { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<BankDetail> BankDetailBanks { get; set; } = new List<BankDetail>();

    public virtual ICollection<BankDetail> BankDetailPaymentModes { get; set; } = new List<BankDetail>();

    public virtual ICollection<BasicProfile> BasicProfiles { get; set; } = new List<BasicProfile>();

    public virtual ICollection<ContactProfile> ContactProfiles { get; set; } = new List<ContactProfile>();

    public virtual ICollection<DocumentProfile> DocumentProfileDocExtensions { get; set; } = new List<DocumentProfile>();

    public virtual ICollection<DocumentProfile> DocumentProfileDocTypeNavigations { get; set; } = new List<DocumentProfile>();

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual ICollection<MasterTypeDetail> InverseParent { get; set; } = new List<MasterTypeDetail>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual MasterTypeDetail? Parent { get; set; }

    public virtual MasterType? Type { get; set; }
}
