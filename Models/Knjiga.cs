using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Knjiga
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Autor { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naslov { get; set; }

        [Required]
        public int BrojOcenjivanja { get; set; }

        [Required]
        [Range(0.00, 5.00)]
        public double Ocena { get; set; }

        public List<Iznajmljivanje> Iznajmljivanja { get; set; }
        
        public List<KnjigaBiblioteka> Biblioteke { get; set; }

        [NotMapped]
        public string ZaPrikaz
        {
            get { return Autor + " - " + Naslov; }
        }
    }
}