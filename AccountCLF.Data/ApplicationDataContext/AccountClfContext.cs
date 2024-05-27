using System;
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

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<DocumentProfile> DocumentProfiles { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MasterLogin> MasterLogins { get; set; }

    public virtual DbSet<MasterType> MasterTypes { get; set; }

    public virtual DbSet<MasterTypeDetail> MasterTypeDetails { get; set; }

    public virtual DbSet<ProfileLink> ProfileLinks { get; set; }

    public virtual DbSet<VoucherSrNo> VoucherSrNos { get; set; }

    public virtual DbSet<VoucherType> VoucherTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=202.66.175.36,1232;database=AccountCLF;user=uSurvey;pwd=Survey@32@12;Encrypt=false;");

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

            entity.HasOne(d => d.Bank).WithMany(p => p.BankDetailBanks)
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

            entity.HasOne(d => d.PaymentMode).WithMany(p => p.BankDetailPaymentModes)
                .HasForeignKey(d => d.PaymentModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankDetail_PaymentMode");
        });

        modelBuilder.Entity<BasicProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BasicPro__3214EC0715ABD588");

            entity.ToTable("BasicProfile");

            entity.Property(e => e.Code).HasMaxLength(20);
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

            entity.Property(e => e.Email)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo).HasMaxLength(200);

            entity.HasOne(d => d.ContactType).WithMany(p => p.ContactProfiles)
                .HasForeignKey(d => d.ContactTypeId)
                .HasConstraintName("FK__ContactPr__Conta__3F466844");

            entity.HasOne(d => d.Entity).WithMany(p => p.ContactProfiles)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__ContactPr__Entit__3E52440B");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Designat__3214EC07E1ABA16C");

            entity.ToTable("Designation");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.DesignationDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Designati__Depar__236943A5");

            entity.HasOne(d => d.DesignationNavigation).WithMany(p => p.DesignationDesignationNavigations)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK__Designati__Desig__22751F6C");

            entity.HasOne(d => d.Entity).WithMany(p => p.Designations)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__Designati__Entit__2180FB33");
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

            entity.HasOne(d => d.AccountType).WithMany(p => p.Entities)
                .HasForeignKey(d => d.AccountTypeId)
                .HasConstraintName("FK__Entity__AccountT__34C8D9D1");

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
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__MasterTyp__Paren__2C3393D0");

            entity.HasOne(d => d.Type).WithMany(p => p.MasterTypeDetails)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__MasterTyp__TypeI__2D27B809");
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

        modelBuilder.Entity<VoucherSrNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VoucherS__3214EC071BECDC64");

            entity.ToTable("VoucherSrNo");

            entity.HasOne(d => d.Clf).WithMany(p => p.VoucherSrNos)
                .HasForeignKey(d => d.ClfId)
                .HasConstraintName("FK__VoucherSr__ClfId__2CF2ADDF");

            entity.HasOne(d => d.VoucherType).WithMany(p => p.VoucherSrNos)
                .HasForeignKey(d => d.VoucherTypeId)
                .HasConstraintName("FK__VoucherSr__Vouch__2BFE89A6");
        });

        modelBuilder.Entity<VoucherType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VoucherT__3214EC07D2E4DADF");

            entity.ToTable("VoucherType");

            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
