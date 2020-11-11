using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class RubahindexTblKompKoor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TblKomponenKoordinat_KoorId_KompId",
                table: "TblKomponenKoordinat");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KoorId",
                table: "TblKomponenKoordinat",
                column: "KoorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TblKomponenKoordinat_KoorId",
                table: "TblKomponenKoordinat");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenKoordinat_KoorId_KompId",
                table: "TblKomponenKoordinat",
                columns: new[] { "KoorId", "KompId" },
                unique: true);
        }
    }
}
