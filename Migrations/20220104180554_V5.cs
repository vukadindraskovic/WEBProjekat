using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_projekat.Migrations
{
    public partial class V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibliotekaKorisnik");

            migrationBuilder.AddColumn<int>(
                name: "BibliotekaID",
                table: "Korisnici",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_BibliotekaID",
                table: "Korisnici",
                column: "BibliotekaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnici_Biblioteke_BibliotekaID",
                table: "Korisnici",
                column: "BibliotekaID",
                principalTable: "Biblioteke",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnici_Biblioteke_BibliotekaID",
                table: "Korisnici");

            migrationBuilder.DropIndex(
                name: "IX_Korisnici_BibliotekaID",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "BibliotekaID",
                table: "Korisnici");

            migrationBuilder.CreateTable(
                name: "BibliotekaKorisnik",
                columns: table => new
                {
                    BibliotekeID = table.Column<int>(type: "int", nullable: false),
                    KorisniciID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibliotekaKorisnik", x => new { x.BibliotekeID, x.KorisniciID });
                    table.ForeignKey(
                        name: "FK_BibliotekaKorisnik_Biblioteke_BibliotekeID",
                        column: x => x.BibliotekeID,
                        principalTable: "Biblioteke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BibliotekaKorisnik_Korisnici_KorisniciID",
                        column: x => x.KorisniciID,
                        principalTable: "Korisnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BibliotekaKorisnik_KorisniciID",
                table: "BibliotekaKorisnik",
                column: "KorisniciID");
        }
    }
}
