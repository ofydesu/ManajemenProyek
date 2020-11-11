using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class TambahKolPos3DTblPola : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Posisi3D",
                table: "TblPola",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posisi3D",
                table: "TblPola");
        }
    }
}
