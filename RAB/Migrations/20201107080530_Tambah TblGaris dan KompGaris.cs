using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class TambahTblGarisdanKompGaris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TblKoordinat_Xid",
                table: "TblKoordinat");

            migrationBuilder.CreateTable(
                name: "TblGaris",
                columns: table => new
                {
                    GarisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AwalId = table.Column<int>(nullable: true),
                    AkhirId = table.Column<int>(nullable: true)
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
                name: "TblKomponenGaris",
                columns: table => new
                {
                    KompGrsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GarisId = table.Column<int>(nullable: false),
                    KompId = table.Column<int>(nullable: false),
                    PosZId = table.Column<int>(nullable: false),
                    PosRelatif = table.Column<int>(nullable: false),
                    PosRelX = table.Column<int>(nullable: false),
                    PosRelY = table.Column<int>(nullable: false),
                    TitikZTtkId = table.Column<int>(nullable: true)
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
                        name: "FK_TblKomponenGaris_TblKomponenPola_KompId",
                        column: x => x.KompId,
                        principalTable: "TblKomponenPola",
                        principalColumn: "KomPolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblKomponenGaris_TblTitik_TitikZTtkId",
                        column: x => x.TitikZTtkId,
                        principalTable: "TblTitik",
                        principalColumn: "TtkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblKoordinat_Xid_Yid",
                table: "TblKoordinat",
                columns: new[] { "Xid", "Yid" },
                unique: true,
                filter: "[Xid] IS NOT NULL AND [Yid] IS NOT NULL");

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
                name: "IX_TblKomponenGaris_KompId",
                table: "TblKomponenGaris",
                column: "KompId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_TitikZTtkId",
                table: "TblKomponenGaris",
                column: "TitikZTtkId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_GarisId_KompId",
                table: "TblKomponenGaris",
                columns: new[] { "GarisId", "KompId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblKomponenGaris");

            migrationBuilder.DropTable(
                name: "TblGaris");

            migrationBuilder.DropIndex(
                name: "IX_TblKoordinat_Xid_Yid",
                table: "TblKoordinat");

            migrationBuilder.CreateIndex(
                name: "IX_TblKoordinat_Xid",
                table: "TblKoordinat",
                column: "Xid");
        }
    }
}
