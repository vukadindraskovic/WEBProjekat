using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Biblioteka
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [Required]
        [MaxLength(50)]
        public string Adresa { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(10)]
        public string Kontakt { get; set; }
        
        public List<Korisnik> Korisnici { get; set; }

        public List<Iznajmljivanje> Iznajmljivanja { get; set; }

        public List<KnjigaBiblioteka> Knjige { get; set; }
    }
}