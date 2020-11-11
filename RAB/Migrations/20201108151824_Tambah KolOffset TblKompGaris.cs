using Microsoft.EntityFrameworkCore.Migrations;

namespace RAB.Migrations
{
    public partial class TambahKolOffsetTblKompGaris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OffsetKeKanan",
                table: "TblKomponenGaris",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OffsetKeKanan",
                table: "TblKomponenGaris");
        }
    }
}
