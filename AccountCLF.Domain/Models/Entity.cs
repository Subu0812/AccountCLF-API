using System;
using System.Collections.Generic;

namespace Model;

public partial class Entity
{
    public int Id { get; set; }

    public int? TypeId { get; set; }

    public DateTime? Date { get; set; }

    public int? AccountTypeId { get; set; }

    public int? SessionId { get; set; }

    public int? ReferenceId { get; set; }

    public int? StaffId { get; set; }

    public int? Status { get; set; }

    public int? IsActive { get; set; }

    public int? EntityTypeId { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public virtual AccountGroup? AccountType { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<BankDetail> BankDetails { get; set; } = new List<BankDetail>();

    public virtual ICollection<BasicProfile> BasicProfiles { get; set; } = new List<BasicProfile>();

    public virtual ICollection<ContactProfile> ContactProfiles { get; set; } = new List<ContactProfile>();

    public virtual ICollection<Daybook> DaybookAccounts { get; set; } = new List<Daybook>();

    public virtual ICollection<Daybook> DaybookFranchises { get; set; } = new List<Daybook>();

    public virtual ICollection<Daybook> DaybookStaffs { get; set; } = new List<Daybook>();

    public virtual ICollection<Designation> DesignationEntities { get; set; } = new List<Designation>();

    public virtual ICollection<Designation> DesignationReferences { get; set; } = new List<Designation>();

    public virtual ICollection<DocumentProfile> DocumentProfiles { get; set; } = new List<DocumentProfile>();

    public virtual EntityType? EntityType { get; set; }

    public virtual ICollection<Entity> InverseParent { get; set; } = new List<Entity>();

    public virtual ICollection<Entity> InverseReference { get; set; } = new List<Entity>();

    public virtual ICollection<Entity> InverseStaff { get; set; } = new List<Entity>();

    public virtual ICollection<LoanAccountDetail> LoanAccountDetails { get; set; } = new List<LoanAccountDetail>();

    public virtual ICollection<LoanAccount> LoanAccounts { get; set; } = new List<LoanAccount>();

    public virtual ICollection<MasterLogin> MasterLogins { get; set; } = new List<MasterLogin>();

    public virtual ICollection<Otp> Otps { get; set; } = new List<Otp>();

    public virtual Entity? Parent { get; set; }

    public virtual ICollection<ProfileLink> ProfileLinks { get; set; } = new List<ProfileLink>();

    public virtual Entity? Reference { get; set; }

    public virtual AccountSession? Session { get; set; }

    public virtual Entity? Staff { get; set; }

    public virtual ICollection<TransFund> TransFundEntities { get; set; } = new List<TransFund>();

    public virtual ICollection<TransFundPaymentDetail> TransFundPaymentDetails { get; set; } = new List<TransFundPaymentDetail>();

    public virtual ICollection<TransFund> TransFundStaffs { get; set; } = new List<TransFund>();

    public virtual MasterTypeDetail? Type { get; set; }

    public virtual ICollection<VoucherSrNo> VoucherSrNos { get; set; } = new List<VoucherSrNo>();
}
