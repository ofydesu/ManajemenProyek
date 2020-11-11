using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class PerbaikanFKTblKomposisiGaris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblKomponenGaris_TblTitik_TitikZTtkId",
                table: "TblKomponenGaris");

            migrationBuilder.DropIndex(
                name: "IX_TblKomponenGaris_TitikZTtkId",
                table: "TblKomponenGaris");

            migrationBuilder.DropIndex(
                name: "IX_TblKomponenGaris_GarisId_KompId",
                table: "TblKomponenGaris");

            migrationBuilder.DropColumn(
                name: "TitikZTtkId",
                table: "TblKomponenGaris");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_PosZId",
                table: "TblKomponenGaris",
                column: "PosZId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_GarisId_KompId_PosRelatif_PosRelX",
                table: "TblKomponenGaris",
                columns: new[] { "GarisId", "KompId", "PosRelatif", "PosRelX" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TblKomponenGaris_TblTitik_PosZId",
                table: "TblKomponenGaris",
                column: "PosZId",
                principalTable: "TblTitik",
                principalColumn: "TtkId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblKomponenGaris_TblTitik_PosZId",
                table: "TblKomponenGaris");

            migrationBuilder.DropIndex(
                name: "IX_TblKomponenGaris_PosZId",
                table: "TblKomponenGaris");

            migrationBuilder.DropIndex(
                name: "IX_TblKomponenGaris_GarisId_KompId_PosRelatif_PosRelX",
                table: "TblKomponenGaris");

            migrationBuilder.AddColumn<int>(
                name: "TitikZTtkId",
                table: "TblKomponenGaris",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_TitikZTtkId",
                table: "TblKomponenGaris",
                column: "TitikZTtkId");

            migrationBuilder.CreateIndex(
                name: "IX_TblKomponenGaris_GarisId_KompId",
                table: "TblKomponenGaris",
                columns: new[] { "GarisId", "KompId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TblKomponenGaris_TblTitik_TitikZTtkId",
                table: "TblKomponenGaris",
                column: "TitikZTtkId",
                principalTable: "TblTitik",
                principalColumn: "TtkId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
