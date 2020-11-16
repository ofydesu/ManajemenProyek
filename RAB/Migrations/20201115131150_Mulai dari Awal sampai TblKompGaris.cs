using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class MulaidariAwalsampaiTblKompGaris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblPola",
                columns: table => new
                {
                    PolaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SatuanPenyusun = table.Column<int>(nullable: false),
                    Posisi3D = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPola", x => x.PolaId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblKomponenPola",
                columns: table => new
                {
                    KomPolaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolaId = table.Column<int>(nullable: true),
                    KompId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKomponenPola", x => x.KomPolaId);
                    table.ForeignKey(
                        name: "FK_TblKomponenPola_TblPola_KompId",
                        column: x => x.KompId,
                        principalTable: "TblPola",
                        principalColumn: "PolaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblKomponenPola_TblPola_PolaId",
                        column: x => x.PolaId,
                        principalTable: "TblPola",
                        principalColumn: "PolaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblTitik",
                columns: table => new
                {
                    TtkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolaId = table.Column<int>(nullable: false),
                    SumbuId = table.Column<int>(nullable: false),
                    Sumbu = table.Column<int>(nullable: false),
                    Nama = table.Column<string>(type: "varchar(50)", nullable: false),
                    Jarak = table.Column<int>(nullable: false),
                    PosAbs = table.Column<int>(nullable: false),
                    PosRel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTitik", x => x.TtkId);
                    table.ForeignKey(
                        name: "FK_TblTitik_TblPola_PolaId",
                        column: x => x.PolaId,
                        principalTable: "TblPola",
                        principalColumn: "PolaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblKoordinat",
                columns: table => new
                {
                    KoordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Xid = table.Column<int>(nullable: true),
                    Yid = table.Column<int>(nullable: true),
                    TidakKeX = table.Column<bool>(nullable: false),
                    SbgAwalX = table.Column<bool>(nullable: false),
                    SbgAwalY = table.Column<bool>(nullable: false),
                    SbgAkhirX = table.Column<bool>(nullable: false),
                    SbgAkhirY = table.Column<bool>(nullable: false),
                    TidakKeY = table.Column<bool>(nullable: false),
                    MiringAtas = table.Column<bool>(nullable: false),
                    MiringBawah = table.Column<bool>(nullable: false),
                    LengkungAtas = table.Column<bool>(nullable: false),
                    LengkungBawah = table.Column<bool>(nullable: false),
                    KiriBawahSemu = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKoordinat", x => x.KoordId);
                    table.ForeignKey(
                        name: "FK_TblKoordinat_TblTitik_Xid",
                        column: x => x.Xid,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblKoordinat_TblTitik_Yid",
                        column: x => x.Yid,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblGaris",
                columns: table => new
                {
                    GarisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AwalId = table.Column<int>(nullable: true),
                    AkhirId = table.Column<int>(nullable: true),
                    Arah = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblGaris", x => x.GarisId);
                    table.ForeignKey(
                        name: "FK_TblGaris_TblKoordinat_AkhirId",
                        column: x => x.AkhirId,
                        principalTable: "TblKoordinat",
                        principalColumn: "KoordId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblGaris_TblKoordinat_AwalId",
                        column: x => x.AwalId,
                        principalTable: "TblKoordinat",
                        principalColumn: "KoordId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblKomponenKoordinat",
                columns: table => new
                {
                    KompKoorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoorId = table.Column<int>(nullable: false),
                    KompId = table.Column<int>(nullable: false),
                    PosAtasId = table.Column<int>(nullable: true),
                    PosBawahId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKomponenKoordinat", x => x.KompKoorId);
                    table.ForeignKey(
                        name: "FK_TblKomponenKoordinat_TblKomponenPola_KompId",
                        column: x => x.KompId,
                        principalTable: "TblKomponenPola",
                        principalColumn: "KomPolaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblKomponenKoordinat_TblKoordinat_KoorId",
                        column: x => x.KoorId,
                        principalTable: "TblKoordinat",
                        principalColumn: "KoordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblKomponenKoordinat_TblTitik_PosAtasId",
                        column: x => x.PosAtasId,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblKomponenKoordinat_TblTitik_PosBawahId",
                        column: x => x.PosBawahId,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblKomponenGaris",
                columns: table => new
                {
                    KompGrsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GarisId = table.Column<int>(nullable: false),
                    KompPolaId = table.Column<int>(nullable: false),
                    PosRelatifDiZ = table.Column<int>(nullable: false),
                    PosZId = table.Column<int>(nullable: false),
                    PosRelatif = table.Column<int>(nullable: false),
                    PosRelX = table.Column<int>(nullable: false),
                    PosRelY = table.Column<int>(nullable: false),
                    OffsetKeKanan = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKomponenGaris", x => x.KompGrsId);
                    table.ForeignKey(
                        name: "FK_TblKomponenGaris_TblGaris_GarisId",
                        column: x => x.GarisId,
                        principalTable: "TblGaris",
                        principalColumn: "GarisId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblKomponenGaris_TblKomponenPola_KompPolaId",
                        column: x => x.KompPolaId,
                        principalTable: "TblKomponenPola",
                        principalColumn: "KomPolaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblKomponenGaris_TblTitik_PosZId",
                        column: x => x.PosZId,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TblGaris_AkhirId",
                table: "TblGaris",
                column: "AkhirId");

            migrationBuilder.CreateIndex(
                name: "IX_TblGaris_AwalId_AkhirId",
                table: "TblGaris",
                columns: new[] { "AwalId", "AkhirId" },
                unique: true,
                filter: "[AwalId] IS NOT NULL AND [AkhirId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_KompPolaId",
                table: "TblKomponenGaris",
                column: "KompPolaId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_PosZId",
                table: "TblKomponenGaris",
                column: "PosZId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_GarisId_KompPolaId_PosRelatif_PosRelX",
                table: "TblKomponenGaris",
                columns: new[] { "GarisId", "KompPolaId", "PosRelatif", "PosRelX" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KompId",
                table: "TblKomponenKoordinat",
                column: "KompId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KoorId",
                table: "TblKomponenKoordinat",
                column: "KoorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_PosAtasId",
                table: "TblKomponenKoordinat",
                column: "PosAtasId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_PosBawahId",
                table: "TblKomponenKoordinat",
                column: "PosBawahId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenPola_KompId",
                table: "TblKomponenPola",
                column: "KompId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenPola_PolaId_KompId",
                table: "TblKomponenPola",
                columns: new[] { "PolaId", "KompId" },
                unique: true,
                filter: "[PolaId] IS NOT NULL AND [KompId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TblKoordinat_Yid",
                table: "TblKoordinat",
                column: "Yid");

            migrationBuilder.CreateIndex(
                name: "IX_TblKoordinat_Xid_Yid",
                table: "TblKoordinat",
                columns: new[] { "Xid", "Yid" },
                unique: true,
                filter: "[Xid] IS NOT NULL AND [Yid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TblPola_Nama",
                table: "TblPola",
                column: "Nama",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblTitik_PolaId_Sumbu_SumbuId",
                table: "TblTitik",
                columns: new[] { "PolaId", "Sumbu", "SumbuId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "TblKomponenGaris");

            migrationBuilder.DropTable(
                name: "TblKomponenKoordinat");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TblGaris");

            migrationBuilder.DropTable(
                name: "TblKomponenPola");

            migrationBuilder.DropTable(
                name: "TblKoordinat");

            migrationBuilder.DropTable(
                name: "TblTitik");

            migrationBuilder.DropTable(
                name: "TblPola");
        }
    }
}
