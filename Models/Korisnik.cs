using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"\d+")]
        [MaxLength(13)]
        [MinLength(13)]
        public string JMBG { get; set; }    

        [Required]
        [MaxLength(30)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(30)]
        public string Prezime { get; set; }

        [JsonIgnore]
        public Biblioteka Biblioteka { get; set; }

        public List<Iznajmljivanje> Iznajmljivanja { get; set; }

        [NotMapped]
        public string ZaPrikaz
        {
            get { return Ime + " " + Prezime + " " + JMBG; }
        }
    }
}