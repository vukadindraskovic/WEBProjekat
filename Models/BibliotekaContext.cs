using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class BibliotekaContext : DbContext
    {
        public DbSet<Biblioteka> Biblioteke { get; set; }
        public DbSet<Knjiga> Knjige { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Iznajmljivanje> Iznajmljivanja { get; set; }
        public DbSet<KnjigaBiblioteka> KnjigeBiblioteke { get; set; }

        public BibliotekaContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}