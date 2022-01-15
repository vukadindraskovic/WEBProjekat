using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class KnjigaBiblioteka
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [JsonIgnore]
        public Knjiga Knjiga { get; set; }

        [Required]
        [JsonIgnore]
        public Biblioteka Biblioteka { get; set; }

        [Range(1, 100)]
        public int Kolicina { get; set; }

        [Range(1, 100)]
        public int Preostalo { get; set; }
    }
}