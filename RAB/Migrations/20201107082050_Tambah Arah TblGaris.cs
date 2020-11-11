using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class TambahArahTblGaris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Arah",
                table: "TblGaris",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arah",
                table: "TblGaris");
        }
    }
}
