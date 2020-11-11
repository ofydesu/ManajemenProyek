using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class TambahTblKompKoord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        principalColumn: "KomPolId",
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

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KompId",
                table: "TblKomponenKoordinat",
                column: "KompId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_PosAtasId",
                table: "TblKomponenKoordinat",
                column: "PosAtasId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_PosBawahId",
                table: "TblKomponenKoordinat",
                column: "PosBawahId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KoorId_KompId",
                table: "TblKomponenKoordinat",
                columns: new[] { "KoorId", "KompId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblKomponenKoordinat");
        }
    }
}
