using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WEBProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KorisnikController : ControllerBase
    {
        public BibliotekaContext Context { get; set; }

        public KorisnikController(BibliotekaContext context)
        {
            Context = context;
        }

        [Route("PreuzmiKorisnikeIzBiblioteke")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiKorisnikeIzBiblioteke([FromQuery] string kontaktBiblioteke)
        {
            if (kontaktBiblioteke is null)
            {
                return BadRequest(new { Poruka = "Morate uneti kontakt biblioteke!"});
            }

            if (kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            try
            {
                 var bibliotekaUBazi = Context.Biblioteke.Where(p => p.Kontakt == kontaktBiblioteke).FirstOrDefault();

                if (bibliotekaUBazi is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var korisnici = await Context.Korisnici
                                            .Include(p => p.Biblioteka)
                                            .Where(p => p.Biblioteka.Kontakt == kontaktBiblioteke)
                                            .ToListAsync();
                            
                return Ok(
                    korisnici.Select(p => 
                    new
                    {
                        Naziv = p.ZaPrikaz
                    }
                    )
                );
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
            
        }

        [Route("DodajKorisnika")]
        [HttpPost]
        public async Task<ActionResult> DodajKorisnika([FromQuery] string kontaktBiblioteke, [FromQuery] string ime, [FromQuery] string prezime, [FromQuery] string JMBG)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (string.IsNullOrWhiteSpace(JMBG) || JMBG.Length != 13)
            {
                return BadRequest(new { Poruka = "JMBG Korisnika mora sadržati 13 cifara!"});
            }

            if (string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešno ime!"});
            }

            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešno prezime!"});
            }

            try
            {
                var biblioteka = Context.Biblioteke.Where(p => p.Kontakt == kontaktBiblioteke).FirstOrDefault();

                if (biblioteka is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var korisnik = Context.Korisnici.Where(p => p.JMBG == JMBG).Include(p => p.Biblioteka).FirstOrDefault();

                if (korisnik != null)
                {
                    return BadRequest(new { Poruka = $"Korisnik sa JMBG '{JMBG}' je već registrovan u biblioteci '{korisnik.Biblioteka.Naziv}'!"});
                }

                var korisnikSaKlijenta = new Korisnik();
                korisnikSaKlijenta.Ime = ime;
                korisnikSaKlijenta.Prezime = prezime;
                korisnikSaKlijenta.JMBG = JMBG;
                korisnikSaKlijenta.Biblioteka = biblioteka;
                Context.Korisnici.Add(korisnikSaKlijenta);
                await Context.SaveChangesAsync();
                return Ok(new { Poruka = $"Korisnik '{korisnikSaKlijenta.ZaPrikaz}' je uspešno dodat u biblioteku '{biblioteka.Naziv}'."});
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }
    }
}