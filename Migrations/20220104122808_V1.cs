using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_projekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Biblioteke",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kontakt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteke", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Knjige",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Autor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrojOcenjivanja = table.Column<int>(type: "int", nullable: false),
                    Ocena = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjige", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KnjigeBiblioteke",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnjigaID = table.Column<int>(type: "int", nullable: false),
                    BibliotekaID = table.Column<int>(type: "int", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnjigeBiblioteke", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KnjigeBiblioteke_Biblioteke_BibliotekaID",
                        column: x => x.BibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnjigeBiblioteke_Knjige_KnjigaID",
                        column: x => x.KnjigaID,
                        principalTable: "Knjige",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Iznajmljivanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    BibliotekaID = table.Column<int>(type: "int", nullable: false),
                    KnjigaID = table.Column<int>(type: "int", nullable: false),
                    KnjigaVracena = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iznajmljivanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Biblioteke_BibliotekaID",
                        column: x => x.BibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Knjige_KnjigaID",
                        column: x => x.KnjigaID,
                        principalTable: "Knjige",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BibliotekaKorisnik_KorisniciID",
                table: "BibliotekaKorisnik",
                column: "KorisniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_BibliotekaID",
                table: "Iznajmljivanja",
                column: "BibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_KnjigaID",
                table: "Iznajmljivanja",
                column: "KnjigaID");

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_KorisnikID",
                table: "Iznajmljivanja",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_KnjigeBiblioteke_BibliotekaID",
                table: "KnjigeBiblioteke",
                column: "BibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_KnjigeBiblioteke_KnjigaID",
                table: "KnjigeBiblioteke",
                column: "KnjigaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibliotekaKorisnik");

            migrationBuilder.DropTable(
                name: "Iznajmljivanja");

            migrationBuilder.DropTable(
                name: "KnjigeBiblioteke");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Biblioteke");

            migrationBuilder.DropTable(
                name: "Knjige");
        }
    }
}
