using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_projekat.Migrations
{
    public partial class V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KnjigaVracena",
                table: "Iznajmljivanja");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "KnjigaVracena",
                table: "Iznajmljivanja",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
