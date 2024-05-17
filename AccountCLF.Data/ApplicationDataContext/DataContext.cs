using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountGroup> AccountGroups { get; set; }

    public virtual DbSet<AccountSession> AccountSessions { get; set; }

    public DbSet<AddressDetail> AddressDetails { get; set; }

    public DbSet<BasicProfile> BasicProfiles { get; set; }

    public DbSet<ContactProfile> ContactProfiles { get; set; }

    public DbSet<DocumentProfile> DocumentProfiles { get; set; }

    public DbSet<Entity> Entities { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<MasterLogin> MasterLogins { get; set; }

    public DbSet<MasterType> MasterTypes { get; set; }

    public DbSet<MasterTypeDetail> MasterTypeDetails { get; set; }

    public DbSet<ProfileLink> ProfileLinks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentProfile>()
            .HasOne(d => d.DocExtension) // Assuming DocExtensionNavigation is the correct navigation property name
            .WithMany() // or .WithOne(), depending on the relationship
            .HasForeignKey(d => d.DocExtensionId)
            .IsRequired(false); // Depending on your requirements

        // If you also have a relationship between DocumentProfile and DocTypeNavigation, configure it similarly
        modelBuilder.Entity<DocumentProfile>()
            .HasOne(d => d.DocTypeNavigation)
            .WithMany()
            .HasForeignKey(d => d.DocType)
            .IsRequired(false);


        modelBuilder.Entity<Entity>()
               .HasOne(e => e.Reference) // Assuming Reference is the correct navigation property name
               .WithMany() // or .WithOne(), depending on the relationship
               .HasForeignKey(e => e.ReferenceId) // Assuming ReferenceId is the foreign key property
               .IsRequired(false);
        modelBuilder.Entity<Entity>()
               .HasOne(e => e.Staff) // Assuming Reference is the correct navigation property name
               .WithMany() // or .WithOne(), depending on the relationship
               .HasForeignKey(e => e.StaffId) // Assuming ReferenceId is the foreign key property
               .IsRequired(false);
    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=202.66.175.36,1232;database=AccountCLF;user=uSurvey;pwd=Survey@32@12;Encrypt=False; ");

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<AccountGroup>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__AccountG__3214EC072C8337E0");

    //            entity.ToTable("AccountGroup");

    //            entity.Property(e => e.InOut)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.NatureType)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.Placcount)
    //                .HasMaxLength(100)
    //                .IsUnicode(false)
    //                .HasColumnName("PLAccount");

    //            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
    //                .HasForeignKey(d => d.ParentId)
    //                .HasConstraintName("FK__AccountGr__Paren__267ABA7A");
    //        });

    //        modelBuilder.Entity<AccountSession>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__AccountS__3214EC07546FE37D");

    //            entity.ToTable("AccountSession");

    //            entity.Property(e => e.Code)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.EndDate).HasColumnType("datetime");
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.StartDate).HasColumnType("datetime");
    //        });

    //        modelBuilder.Entity<AddressDetail>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__AddressD__3214EC073E610A60");

    //            entity.ToTable("AddressDetail");

    //            entity.Property(e => e.Address)
    //                .HasMaxLength(1000)
    //                .IsUnicode(false);
    //            entity.Property(e => e.LandMark)
    //                .HasMaxLength(500)
    //                .IsUnicode(false);
    //            entity.Property(e => e.PinCode)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);

    //            entity.HasOne(d => d.AddressType).WithMany(p => p.AddressDetails)
    //                .HasForeignKey(d => d.AddressTypeId)
    //                .HasConstraintName("FK__AddressDe__Addre__4316F928");

    //            entity.HasOne(d => d.City).WithMany(p => p.AddressDetails)
    //                .HasForeignKey(d => d.CityId)
    //                .HasConstraintName("FK__AddressDe__CityI__440B1D61");

    //            entity.HasOne(d => d.Entity).WithMany(p => p.AddressDetails)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__AddressDe__Entit__4222D4EF");
    //        });

    //        modelBuilder.Entity<BasicProfile>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__BasicPro__3214EC0715ABD588");

    //            entity.ToTable("BasicProfile");

    //            entity.Property(e => e.Code).HasMaxLength(20);
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(200)
    //                .IsUnicode(false);

    //            entity.HasOne(d => d.DesignationNavigation).WithMany(p => p.BasicProfiles)
    //                .HasForeignKey(d => d.Designation)
    //                .HasConstraintName("FK__BasicProf__Desig__3B75D760");

    //            entity.HasOne(d => d.Entity).WithMany(p => p.BasicProfiles)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__BasicProf__Entit__3A81B327");
    //        });

    //        modelBuilder.Entity<ContactProfile>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__ContactP__3214EC07EBC7DC23");

    //            entity.ToTable("ContactProfile");

    //            entity.Property(e => e.Email)
    //                .HasMaxLength(300)
    //                .IsUnicode(false);
    //            entity.Property(e => e.MobileNo).HasMaxLength(200);

    //            entity.HasOne(d => d.ContactType).WithMany(p => p.ContactProfiles)
    //                .HasForeignKey(d => d.ContactTypeId)
    //                .HasConstraintName("FK__ContactPr__Conta__3F466844");

    //            entity.HasOne(d => d.Entity).WithMany(p => p.ContactProfiles)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__ContactPr__Entit__3E52440B");
    //        });

    //        modelBuilder.Entity<DocumentProfile>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC070AD8C9AB");

    //            entity.ToTable("DocumentProfile");

    //            entity.Property(e => e.AltTag)
    //                .HasMaxLength(300)
    //                .IsUnicode(false);
    //            entity.Property(e => e.Description)
    //                .HasMaxLength(1000)
    //                .IsUnicode(false);
    //            entity.Property(e => e.InsDate).HasColumnType("datetime");
    //            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

    //            entity.HasOne(d => d.DocExtension).WithMany(p => p.DocumentProfileDocExtensions)
    //                .HasForeignKey(d => d.DocExtensionId)
    //                .HasConstraintName("FK__DocumentP__DocEx__48CFD27E");

    //            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.DocumentProfileDocTypeNavigations)
    //                .HasForeignKey(d => d.DocType)
    //                .HasConstraintName("FK__DocumentP__DocTy__47DBAE45");

    //            entity.HasOne(d => d.Entity).WithMany(p => p.DocumentProfiles)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__DocumentP__Entit__46E78A0C");
    //        });

    //        modelBuilder.Entity<Entity>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Entity__3214EC07982FC30C");

    //            entity.ToTable("Entity");

    //            entity.Property(e => e.Date).HasColumnType("datetime");

    //            entity.HasOne(d => d.AccountType).WithMany(p => p.Entities)
    //                .HasForeignKey(d => d.AccountTypeId)
    //                .HasConstraintName("FK__Entity__AccountT__34C8D9D1");

    //            entity.HasOne(d => d.Reference).WithMany(p => p.InverseReference)
    //                .HasForeignKey(d => d.ReferenceId)
    //                .HasConstraintName("FK__Entity__Referenc__36B12243");

    //            entity.HasOne(d => d.Session).WithMany(p => p.Entities)
    //                .HasForeignKey(d => d.SessionId)
    //                .HasConstraintName("FK__Entity__SessionI__35BCFE0A");

    //            entity.HasOne(d => d.Staff).WithMany(p => p.InverseStaff)
    //                .HasForeignKey(d => d.StaffId)
    //                .HasConstraintName("FK__Entity__StaffId__37A5467C");

    //            entity.HasOne(d => d.Type).WithMany(p => p.Entities)
    //                .HasForeignKey(d => d.TypeId)
    //                .HasConstraintName("FK__Entity__TypeId__33D4B598");
    //        });

    //        modelBuilder.Entity<Location>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0736EBBB70");

    //            entity.ToTable("Location");

    //            entity.Property(e => e.Code).HasMaxLength(20);
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(500)
    //                .IsUnicode(false);
    //            entity.Property(e => e.ShortName).HasMaxLength(20);
    //            entity.Property(e => e.SrNo).HasMaxLength(20);

    //            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
    //                .HasForeignKey(d => d.ParentId)
    //                .HasConstraintName("FK__Location__Parent__300424B4");

    //            entity.HasOne(d => d.Type).WithMany(p => p.Locations)
    //                .HasForeignKey(d => d.TypeId)
    //                .HasConstraintName("FK__Location__TypeId__30F848ED");
    //        });

    //        modelBuilder.Entity<MasterLogin>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__MasterLo__3214EC07FC7460E6");

    //            entity.ToTable("MasterLogin");

    //            entity.Property(e => e.Password).HasMaxLength(500);
    //            entity.Property(e => e.UserName).HasMaxLength(500);

    //            entity.HasOne(d => d.Entity).WithMany(p => p.MasterLogins)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__MasterLog__Entit__5070F446");
    //        });

    //        modelBuilder.Entity<MasterType>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__MasterTy__3214EC078116CBF5");

    //            entity.ToTable("MasterType");

    //            entity.Property(e => e.Date).HasColumnType("datetime");
    //            entity.Property(e => e.Name).HasMaxLength(200);
    //            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

    //            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
    //                .HasForeignKey(d => d.ParentId)
    //                .HasConstraintName("FK__MasterTyp__Paren__29572725");
    //        });

    //        modelBuilder.Entity<MasterTypeDetail>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__MasterTy__3214EC07AA28EE40");

    //            entity.ToTable("MasterTypeDetail");

    //            entity.Property(e => e.Code).HasMaxLength(200);
    //            entity.Property(e => e.Date).HasColumnType("datetime");
    //            entity.Property(e => e.Name).HasMaxLength(500);
    //            entity.Property(e => e.SrNo).HasColumnType("decimal(18, 2)");

    //            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
    //                .HasForeignKey(d => d.ParentId)
    //                .HasConstraintName("FK__MasterTyp__Paren__2C3393D0");

    //            entity.HasOne(d => d.Type).WithMany(p => p.MasterTypeDetails)
    //                .HasForeignKey(d => d.TypeId)
    //                .HasConstraintName("FK__MasterTyp__TypeI__2D27B809");
    //        });

    //        modelBuilder.Entity<ProfileLink>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__ProfileL__3214EC070D226381");

    //            entity.ToTable("ProfileLink");

    //            entity.HasOne(d => d.Entity).WithMany(p => p.ProfileLinkEntities)
    //                .HasForeignKey(d => d.EntityId)
    //                .HasConstraintName("FK__ProfileLi__Entit__4BAC3F29");

    //            entity.HasOne(d => d.FatherNameNavigation).WithMany(p => p.ProfileLinkFatherNameNavigations)
    //                .HasForeignKey(d => d.FatherName)
    //                .HasConstraintName("FK__ProfileLi__Fathe__4CA06362");

    //            entity.HasOne(d => d.MotherNameNavigation).WithMany(p => p.ProfileLinkMotherNameNavigations)
    //                .HasForeignKey(d => d.MotherName)
    //                .HasConstraintName("FK__ProfileLi__Mothe__4D94879B");
    //        });

    //        OnModelCreatingPartial(modelBuilder);
    //    }

    //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
