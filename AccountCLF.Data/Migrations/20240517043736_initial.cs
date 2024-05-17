using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountCLF.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    InOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placcount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountGroups_AccountGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsShow = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrNo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTypes_MasterTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MasterTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MasterTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrNo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTypeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTypeDetails_MasterTypeDetails_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MasterTypeDetails_MasterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MasterTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    SessionId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entities_AccountGroups_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entities_AccountSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "AccountSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entities_Entities_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entities_Entities_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entities_MasterTypeDetails_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_MasterTypeDetails_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BasicProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<int>(type: "int", nullable: true),
                    DesignationNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicProfiles_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BasicProfiles_MasterTypeDetails_DesignationNavigationId",
                        column: x => x.DesignationNavigationId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ContactTypeId = table.Column<int>(type: "int", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactProfiles_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactProfiles_MasterTypeDetails_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrNo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    DocType = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocExtensionId = table.Column<int>(type: "int", nullable: true),
                    AltTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true),
                    MasterTypeDetailId = table.Column<int>(type: "int", nullable: true),
                    MasterTypeDetailId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentProfiles_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentProfiles_MasterTypeDetails_DocExtensionId",
                        column: x => x.DocExtensionId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentProfiles_MasterTypeDetails_DocType",
                        column: x => x.DocType,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentProfiles_MasterTypeDetails_MasterTypeDetailId",
                        column: x => x.MasterTypeDetailId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentProfiles_MasterTypeDetails_MasterTypeDetailId1",
                        column: x => x.MasterTypeDetailId1,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityEntity",
                columns: table => new
                {
                    InverseReferenceId = table.Column<int>(type: "int", nullable: false),
                    InverseStaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityEntity", x => new { x.InverseReferenceId, x.InverseStaffId });
                    table.ForeignKey(
                        name: "FK_EntityEntity_Entities_InverseReferenceId",
                        column: x => x.InverseReferenceId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityEntity_Entities_InverseStaffId",
                        column: x => x.InverseStaffId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MasterLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterLogins_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProfileLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileLinks_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AddressDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    AddressTypeId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandMark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressDetails_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddressDetails_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddressDetails_MasterTypeDetails_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalTable: "MasterTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_ParentId",
                table: "AccountGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressDetails_AddressTypeId",
                table: "AddressDetails",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressDetails_CityId",
                table: "AddressDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressDetails_EntityId",
                table: "AddressDetails",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicProfiles_DesignationNavigationId",
                table: "BasicProfiles",
                column: "DesignationNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicProfiles_EntityId",
                table: "BasicProfiles",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactProfiles_ContactTypeId",
                table: "ContactProfiles",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactProfiles_EntityId",
                table: "ContactProfiles",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProfiles_DocExtensionId",
                table: "DocumentProfiles",
                column: "DocExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProfiles_DocType",
                table: "DocumentProfiles",
                column: "DocType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProfiles_EntityId",
                table: "DocumentProfiles",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProfiles_MasterTypeDetailId",
                table: "DocumentProfiles",
                column: "MasterTypeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProfiles_MasterTypeDetailId1",
                table: "DocumentProfiles",
                column: "MasterTypeDetailId1");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_AccountTypeId",
                table: "Entities",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ReferenceId",
                table: "Entities",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_SessionId",
                table: "Entities",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_StaffId",
                table: "Entities",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_TypeId",
                table: "Entities",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityEntity_InverseStaffId",
                table: "EntityEntity",
                column: "InverseStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentId",
                table: "Locations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TypeId",
                table: "Locations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterLogins_EntityId",
                table: "MasterLogins",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTypeDetails_ParentId",
                table: "MasterTypeDetails",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTypeDetails_TypeId",
                table: "MasterTypeDetails",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTypes_ParentId",
                table: "MasterTypes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLinks_EntityId",
                table: "ProfileLinks",
                column: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressDetails");

            migrationBuilder.DropTable(
                name: "BasicProfiles");

            migrationBuilder.DropTable(
                name: "ContactProfiles");

            migrationBuilder.DropTable(
                name: "DocumentProfiles");

            migrationBuilder.DropTable(
                name: "EntityEntity");

            migrationBuilder.DropTable(
                name: "MasterLogins");

            migrationBuilder.DropTable(
                name: "ProfileLinks");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Entities");

            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropTable(
                name: "AccountSessions");

            migrationBuilder.DropTable(
                name: "MasterTypeDetails");

            migrationBuilder.DropTable(
                name: "MasterTypes");
        }
    }
}
