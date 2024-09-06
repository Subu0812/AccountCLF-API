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

    public DateTime? Date { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<BankDetail> BankDetailBanks { get; set; } = new List<BankDetail>();

    public virtual ICollection<BankDetail> BankDetailPaymentModes { get; set; } = new List<BankDetail>();

    public virtual ICollection<ContactProfile> ContactProfiles { get; set; } = new List<ContactProfile>();

    public virtual ICollection<Designation> DesignationDepartments { get; set; } = new List<Designation>();

    public virtual ICollection<Designation> DesignationDesignationNavigations { get; set; } = new List<Designation>();

    public virtual ICollection<DocumentProfile> DocumentProfileDocExtensions { get; set; } = new List<DocumentProfile>();

    public virtual ICollection<DocumentProfile> DocumentProfileDocTypeNavigations { get; set; } = new List<DocumentProfile>();

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual ICollection<MasterTypeDetail> InverseParent { get; set; } = new List<MasterTypeDetail>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual MasterTypeDetail? Parent { get; set; }

    public virtual ICollection<TransFundBillingDetail> TransFundBillingDetails { get; set; } = new List<TransFundBillingDetail>();

    public virtual ICollection<TransFundDetail> TransFundDetails { get; set; } = new List<TransFundDetail>();

    public virtual ICollection<TransFund> TransFundFranchises { get; set; } = new List<TransFund>();

    public virtual ICollection<TransFund> TransFundLedgerHeads { get; set; } = new List<TransFund>();

    public virtual ICollection<TransFund> TransFundPayModeNavigations { get; set; } = new List<TransFund>();

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetailBanks { get; set; } = new List<TransFundPaymentDetail>();

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetailPaymentModes { get; set; } = new List<TransFundPaymentDetail>();

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetailTransTypeNavigations { get; set; } = new List<TransFundPaymentDetail>();

    public virtual MasterType? Type { get; set; }
}
