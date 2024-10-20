﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public partial class AccountClfContext : DbContext
{
    public AccountClfContext()
    {
    }

    public AccountClfContext(DbContextOptions<AccountClfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountGroup> AccountGroups { get; set; }

    public virtual DbSet<AccountSession> AccountSessions { get; set; }

    public virtual DbSet<AddressDetail> AddressDetails { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<BasicProfile> BasicProfiles { get; set; }

    public virtual DbSet<ContactProfile> ContactProfiles { get; set; }

    public virtual DbSet<Daybook> Daybooks { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<DocumentProfile> DocumentProfiles { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<LoanAccount> LoanAccounts { get; set; }

    public virtual DbSet<LoanAccountDetail> LoanAccountDetails { get; set; }

    public virtual DbSet<LoanInterest> LoanInterests { get; set; }

    public virtual DbSet<LoanTenure> LoanTenures { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MasterLogin> MasterLogins { get; set; }

    public virtual DbSet<MasterType> MasterTypes { get; set; }

    public virtual DbSet<MasterTypeDetail> MasterTypeDetails { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<ProfileLink> ProfileLinks { get; set; }

    public virtual DbSet<TransFund> TransFunds { get; set; }

    public virtual DbSet<TransFundBill> TransFundBills { get; set; }

    public virtual DbSet<TransFundBillingDetail> TransFundBillingDetails { get; set; }

    public virtual DbSet<TransFundDetail> TransFundDetails { get; set; }

    public virtual DbSet<TransFundGst> TransFundGsts { get; set; }

    public virtual DbSet<TransFundLink> TransFundLinks { get; set; }

    public virtual DbSet<TransFundPaymentDetail> TransFundPaymentDetails { get; set; }

    public virtual DbSet<TransFundRemark> TransFundRemarks { get; set; }

    public virtual DbSet<TransFundTd> TransFundTds { get; set; }

    public virtual DbSet<VoucherSrNo> VoucherSrNos { get; set; }

    public virtual DbSet<VoucherType> VoucherTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=160.187.54.51,1232;database=AccountCLF;user=uSurvey;pwd=Survey@32@12;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountG__3214EC072C8337E0");

            entity.ToTable("AccountGroup");

            entity.Property(e => e.InOut)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NatureType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Placcount)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PLAccount");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__AccountGr__Paren__267ABA7A");
        });

        modelBuilder.Entity<AccountSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountS__3214EC07546FE37D");

            entity.ToTable("AccountSession");

            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<AddressDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AddressD__3214EC073E610A60");

            entity.ToTable("AddressDetail");

            entity.Property(e => e.Address)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LandMark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressType).WithMany(p => p.AddressDetails)
                .HasForeignKey(d => d.AddressTypeId)
                .HasConstraintName("FK__AddressDe__Addre__4316F928");

            entity.HasOne(d => d.City).WithMany(p => p.AddressDetails)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__AddressDe__CityI__440B1D61");

            entity.HasOne(d => d.Entity).WithMany(p => p.AddressDetails)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__AddressDe__Entit__4222D4EF");
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankDeta__3214EC079C43A7C6");

            entity.ToTable("BankDetail");

            entity.Property(e => e.AccountNo).HasMaxLength(50);
            entity.Property(e => e.BeneficiaryName).HasMaxLength(255);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(11)
                .HasColumnName("IFSCCode");

            entity.HasOne(d => d.Bank).WithMany(p => p.BankDetails)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankDetail_Bank");

            entity.HasOne(d => d.Entity).WithMany(p => p.BankDetails)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankDetail_Entity");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_BankDetail_Parent");
        });

        modelBuilder.Entity<BasicProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BasicPro__3214EC0715ABD588");

            entity.ToTable("BasicProfile");

            entity.Property(e => e.Code).HasMaxLength(300);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Designation).WithMany(p => p.BasicProfiles)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK_BasicProfile_DesignationId");

            entity.HasOne(d => d.Entity).WithMany(p => p.BasicProfiles)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__BasicProf__Entit__3A81B327");
        });

        modelBuilder.Entity<ContactProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactP__3214EC07EBC7DC23");

            entity.ToTable("ContactProfile");

            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.MobileNo).HasMaxLength(200);

            entity.HasOne(d => d.ContactType).WithMany(p => p.ContactProfiles)
                .HasForeignKey(d => d.ContactTypeId)
                .HasConstraintName("FK__ContactPr__Conta__3F466844");

            entity.HasOne(d => d.Entity).WithMany(p => p.ContactProfiles)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__ContactPr__Entit__3E52440B");
        });

        modelBuilder.Entity<Daybook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Daybook__3214EC07263BEDBC");

            entity.ToTable("Daybook");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransType)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DaybookAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Daybook_Entity");

            entity.HasOne(d => d.Franchise).WithMany(p => p.DaybookFranchises)
                .HasForeignKey(d => d.FranchiseId)
                .HasConstraintName("FK__Daybook__Franchi__6442E2C9");

            entity.HasOne(d => d.FundReference).WithMany(p => p.Daybooks)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__Daybook__FundRef__625A9A57");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Daybook_ParentId");

            entity.HasOne(d => d.Session).WithMany(p => p.Daybooks)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Daybook__Session__662B2B3B");

            entity.HasOne(d => d.Staff).WithMany(p => p.DaybookStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Daybook__StaffId__65370702");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Designat__3214EC07E1ABA16C");

            entity.ToTable("Designation");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.DesignationNavigation).WithMany(p => p.Designations)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK__Designati__Desig__22751F6C");

            entity.HasOne(d => d.Entity).WithMany(p => p.DesignationEntities)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__Designati__Entit__2180FB33");

            entity.HasOne(d => d.Reference).WithMany(p => p.DesignationReferences)
                .HasForeignKey(d => d.ReferenceId)
                .HasConstraintName("FK_Designation_Entity");
        });

        modelBuilder.Entity<DocumentProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC070AD8C9AB");

            entity.ToTable("DocumentProfile");

            entity.Property(e => e.AltTag)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.DocExtension).WithMany(p => p.DocumentProfileDocExtensions)
                .HasForeignKey(d => d.DocExtensionId)
                .HasConstraintName("FK__DocumentP__DocEx__48CFD27E");

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.DocumentProfileDocTypeNavigations)
                .HasForeignKey(d => d.DocType)
                .HasConstraintName("FK__DocumentP__DocTy__47DBAE45");

            entity.HasOne(d => d.Entity).WithMany(p => p.DocumentProfiles)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__DocumentP__Entit__46E78A0C");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entity__3214EC07982FC30C");

            entity.ToTable("Entity");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.AccountType).WithMany(p => p.Entities)
                .HasForeignKey(d => d.AccountTypeId)
                .HasConstraintName("FK__Entity__AccountT__34C8D9D1");

            entity.HasOne(d => d.EntityType).WithMany(p => p.Entities)
                .HasForeignKey(d => d.EntityTypeId)
                .HasConstraintName("FK_Entity_EntityType");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Entity_ParentId");

            entity.HasOne(d => d.Reference).WithMany(p => p.InverseReference)
                .HasForeignKey(d => d.ReferenceId)
                .HasConstraintName("FK__Entity__Referenc__36B12243");

            entity.HasOne(d => d.Session).WithMany(p => p.Entities)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Entity__SessionI__35BCFE0A");

            entity.HasOne(d => d.Staff).WithMany(p => p.InverseStaff)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Entity__StaffId__37A5467C");

            entity.HasOne(d => d.Type).WithMany(p => p.Entities)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Entity__TypeId__33D4B598");
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EntityTy__3214EC070550F999");

            entity.ToTable("EntityType");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<LoanAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanAcco__3214EC076837C271");

            entity.ToTable("LoanAccount");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoantenureId).HasColumnName("LoantenureID");
            entity.Property(e => e.PayableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SrNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Entity).WithMany(p => p.LoanAccounts)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK_LoanAccount_Entity");

            entity.HasOne(d => d.FundReference).WithMany(p => p.LoanAccounts)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK_LoanAccount_TransFund");

            entity.HasOne(d => d.Loantenure).WithMany(p => p.LoanAccounts)
                .HasForeignKey(d => d.LoantenureId)
                .HasConstraintName("FK_LoanTenure");
        });

        modelBuilder.Entity<LoanAccountDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanAcco__3214EC078062F601");

            entity.ToTable("LoanAccountDetail");

            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.InterestAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InterestPercentage).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Entity).WithMany(p => p.LoanAccountDetails)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK_LoanAccountDetail_Entity");

            entity.HasOne(d => d.FundReference).WithMany(p => p.LoanAccountDetails)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK_LoanAccountDetail_TransFund");

            entity.HasOne(d => d.LoanAccount).WithMany(p => p.LoanAccountDetails)
                .HasForeignKey(d => d.LoanAccountId)
                .HasConstraintName("FK_LoanAccountDetail_LoanAccount");
        });

        modelBuilder.Entity<LoanInterest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanInte__3214EC07A4BF28DA");

            entity.ToTable("LoanInterest");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<LoanTenure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanTenu__3214EC07EABA17A2");

            entity.ToTable("LoanTenure");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0736EBBB70");

            entity.ToTable("Location");

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ShortName).HasMaxLength(20);
            entity.Property(e => e.SrNo).HasMaxLength(20);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Location__Parent__300424B4");

            entity.HasOne(d => d.Type).WithMany(p => p.Locations)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Location__TypeId__30F848ED");
        });

        modelBuilder.Entity<MasterLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MasterLo__3214EC07FC7460E6");

            entity.ToTable("MasterLogin");

            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(500);

            entity.HasOne(d => d.Entity).WithMany(p => p.MasterLogins)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__MasterLog__Entit__5070F446");
        });

        modelBuilder.Entity<MasterType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MasterTy__3214EC078116CBF5");

            entity.ToTable("MasterType");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__MasterTyp__Paren__29572725");
        });

        modelBuilder.Entity<MasterTypeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MasterTy__3214EC07AA28EE40");

            entity.ToTable("MasterTypeDetail");

            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__MasterTyp__Paren__2C3393D0");

            entity.HasOne(d => d.Type).WithMany(p => p.MasterTypeDetails)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__MasterTyp__TypeI__2D27B809");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OTP__3214EC0794936533");

            entity.ToTable("OTP");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpirationTime).HasColumnType("datetime");
            entity.Property(e => e.ForOtp).HasMaxLength(255);
            entity.Property(e => e.NewMobile).HasMaxLength(255);
            entity.Property(e => e.Otp1).HasColumnName("Otp");

            entity.HasOne(d => d.Entity).WithMany(p => p.Otps)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OTP_Entity");
        });

        modelBuilder.Entity<ProfileLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProfileL__3214EC070D226381");

            entity.ToTable("ProfileLink");

            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.MotherName).HasMaxLength(100);

            entity.HasOne(d => d.Entity).WithMany(p => p.ProfileLinks)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__ProfileLi__Entit__4BAC3F29");
        });

        modelBuilder.Entity<TransFund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC07E8770E95");

            entity.ToTable("TransFund");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentConfirmDate).HasColumnType("datetime");
            entity.Property(e => e.SlipUpload).IsUnicode(false);
            entity.Property(e => e.TaxableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Entity).WithMany(p => p.TransFundEntities)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__TransFund__Entit__503BEA1C");

            entity.HasOne(d => d.Franchise).WithMany(p => p.TransFundFranchises)
                .HasForeignKey(d => d.FranchiseId)
                .HasConstraintName("FK_TransFund_FranchiseID_Entity");

            entity.HasOne(d => d.LedgerHead).WithMany(p => p.TransFundLedgerHeads)
                .HasForeignKey(d => d.LedgerHeadId)
                .HasConstraintName("FK_TransFund_MasterTypeDetail");

            entity.HasOne(d => d.PayModeNavigation).WithMany(p => p.TransFundPayModeNavigations)
                .HasForeignKey(d => d.PayMode)
                .HasConstraintName("FK__TransFund__PayMo__540C7B00");

            entity.HasOne(d => d.Session).WithMany(p => p.TransFunds)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__TransFund__Sessi__51300E55");

            entity.HasOne(d => d.Staff).WithMany(p => p.TransFundStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__TransFund__Staff__531856C7");

            entity.HasOne(d => d.VoucherNoNavigation).WithMany(p => p.TransFunds)
                .HasForeignKey(d => d.VoucherNo)
                .HasConstraintName("FK_transvoucherno_mastertype");

            entity.HasOne(d => d.VoucherTypeNavigation).WithMany(p => p.TransFunds)
                .HasForeignKey(d => d.VoucherType)
                .HasConstraintName("FK_transfund_mastertype");
        });

        modelBuilder.Entity<TransFundBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC07EC7EBEE8");

            entity.ToTable("TransFundBill");

            entity.Property(e => e.BillDate).HasColumnType("datetime");
            entity.Property(e => e.BillNo).HasMaxLength(50);

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundBills)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__FundR__5AB9788F");
        });

        modelBuilder.Entity<TransFundBillingDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC07AB3B9C07");

            entity.ToTable("TransFundBillingDetail");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.ContactDetail)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.LandMark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptCoName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressType).WithMany(p => p.TransFundBillingDetails)
                .HasForeignKey(d => d.AddressTypeId)
                .HasConstraintName("FK__TransFund__Addre__5F7E2DAC");

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundBillingDetails)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__FundR__5D95E53A");

            entity.HasOne(d => d.Location).WithMany(p => p.TransFundBillingDetails)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__TransFund__Locat__5E8A0973");
        });

        modelBuilder.Entity<TransFundDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC07194D303D");

            entity.ToTable("TransFundDetail");

            entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Concession).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPer).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RoundOff).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RoundOperator)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.ConcessionNavigation).WithMany(p => p.TransFundDetails)
                .HasForeignKey(d => d.ConcessionId)
                .HasConstraintName("FK__TransFund__Conce__69FBBC1F");

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundDetails)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__FundR__690797E6");
        });

        modelBuilder.Entity<TransFundGst>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TransFundGST");

            entity.Property(e => e.Cgst)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CGST");
            entity.Property(e => e.Gstno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTNo");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Igst)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("IGST");
            entity.Property(e => e.Sgst)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SGST");
            entity.Property(e => e.TaxPer).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.FundReference).WithMany()
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__FundR__6BE40491");
        });

        modelBuilder.Entity<TransFundLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC07579A38A9");

            entity.ToTable("TransFundLink");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundLinks)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__FundR__6EC0713C");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__TransFund__Paren__6FB49575");
        });

        modelBuilder.Entity<TransFundPaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TransFundPaymentDetail_Id");

            entity.ToTable("TransFundPaymentDetail");

            entity.Property(e => e.ApplyDate).HasColumnType("datetime");
            entity.Property(e => e.BankReferenceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankReferenceId)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BankReferenceStatus)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PayDate).HasColumnType("datetime");
            entity.Property(e => e.ReciptNo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UploadedDoc)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Bank).WithMany(p => p.TransFundPaymentDetailBanks)
                .HasForeignKey(d => d.BankId)
                .HasConstraintName("FK__TransFund__BankI__73852659");

            entity.HasOne(d => d.Daybook).WithMany(p => p.TransFundPaymentDetails)
                .HasForeignKey(d => d.DaybookId)
                .HasConstraintName("FK__TransFund__Daybo__72910220");

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundPaymentDetails)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__PayDa__719CDDE7");

            entity.HasOne(d => d.Ledger).WithMany(p => p.TransFundPaymentDetails)
                .HasForeignKey(d => d.LedgerId)
                .HasConstraintName("FK__TransFund__Ledge__74794A92");

            entity.HasOne(d => d.PaymentMode).WithMany(p => p.TransFundPaymentDetailPaymentModes)
                .HasForeignKey(d => d.PaymentModeId)
                .HasConstraintName("FK__TransFund__Payme__756D6ECB");

            entity.HasOne(d => d.TransTypeNavigation).WithMany(p => p.TransFundPaymentDetailTransTypeNavigations)
                .HasForeignKey(d => d.TransType)
                .HasConstraintName("FK_TransFundPaymentDetail_TransType");
        });

        modelBuilder.Entity<TransFundRemark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TransFun__3214EC077BDA2613");

            entity.ToTable("TransFundRemark");

            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundRemarks)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__Remar__57DD0BE4");
        });

        modelBuilder.Entity<TransFundTd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TransFundTDS_Id");

            entity.ToTable("TransFundTDS");

            entity.Property(e => e.PanNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tds)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TDS");
            entity.Property(e => e.TdsableAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TDSableAmount");
            entity.Property(e => e.Tdsper)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TDSPer");

            entity.HasOne(d => d.FundReference).WithMany(p => p.TransFundTds)
                .HasForeignKey(d => d.FundReferenceId)
                .HasConstraintName("FK__TransFund__PanNo__7755B73D");

            entity.HasOne(d => d.Section).WithMany(p => p.TransFundTds)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_TransFundTds_MasterTypeDetail_SectionId");
        });

        modelBuilder.Entity<VoucherSrNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VoucherS__3214EC071BECDC64");

            entity.ToTable("VoucherSrNo");

            entity.HasOne(d => d.Clf).WithMany(p => p.VoucherSrNos)
                .HasForeignKey(d => d.ClfId)
                .HasConstraintName("FK__VoucherSr__ClfId__2CF2ADDF");

            entity.HasOne(d => d.Session).WithMany(p => p.VoucherSrNos)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK_vouchersrno_SessionId");

            entity.HasOne(d => d.VoucherType).WithMany(p => p.VoucherSrNos)
                .HasForeignKey(d => d.VoucherTypeId)
                .HasConstraintName("FK__VoucherSr__Vouch__2BFE89A6");
        });

        modelBuilder.Entity<VoucherType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VoucherT__3214EC07D2E4DADF");

            entity.ToTable("VoucherType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
