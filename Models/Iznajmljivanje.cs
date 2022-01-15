using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Iznajmljivanje
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [JsonIgnore]
        public Korisnik Korisnik { get; set; }

        [Required]
        [JsonIgnore]
        public Biblioteka Biblioteka { get; set; }

        [Required]
        [JsonIgnore]
        public Knjiga Knjiga { get; set; }

        //[Required]
        //public bool KnjigaVracena { get; set; }
    }
}