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
    public class IznajmljivanjeController : ControllerBase
    {
        public BibliotekaContext Context { get; set; }

        public IznajmljivanjeController(BibliotekaContext context)
        {
            Context = context;
        }

        [Route("IznajmiKnjigu")]
        [HttpPost]
        public async Task<ActionResult> IznajmiKnjigu([FromQuery] string kontaktBiblioteke, [FromQuery] string nazivKorisnika, [FromQuery] string nazivKnjige)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKorisnika) || nazivKorisnika.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešan korisnik!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKnjige) || nazivKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešna knjiga!"});
            }

            // slucaj gde biblioteka ne postoji
            // slucaj gde korisnik ne postoji
            // slucaj gde korisnik ne pripada toj biblioteci
            // slucaj gde knjiga ne postoji
            // slucaj gde knjiga ne pripada toj biblioteci

            try
            {
                var bibliotekaUpit = Context.Biblioteke.Where(p => p.Kontakt == kontaktBiblioteke);

                var biblioteka1 = bibliotekaUpit.FirstOrDefault();

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji."});
                }

                var korisnikUpit = Context.Korisnici.Where(p => p.Ime + " " + p.Prezime + " " + p.JMBG == nazivKorisnika);

                var korisnik1 = korisnikUpit.FirstOrDefault();

                if (korisnik1 is null)
                {
                    return BadRequest(new { Poruka = $"Korisnik '{nazivKorisnika}' ne postoji."});
                }

                var korisnik2 = korisnikUpit.Include(p => p.Biblioteka).FirstOrDefault();

                if (korisnik2.Biblioteka.Kontakt != kontaktBiblioteke)
                {
                    return BadRequest(new { Poruka = $"Korisnik '{korisnik2.ZaPrikaz}' ne pripada biblioteci '{biblioteka1.Naziv}', vec '{korisnik2.Biblioteka.Naziv}'."});
                }

                var knjigaUpit = Context.Knjige.Where(p => p.Autor + " - " + p.Naslov == nazivKnjige);
                var knjiga1 = knjigaUpit.FirstOrDefault();

                if (knjiga1 is null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{nazivKnjige}' ne postoji!"});
                }

                var knjigaBibliotekaUpit = Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                                    .Where(p => p.Biblioteka == biblioteka1)
                                                                    .Include(p => p.Knjiga)
                                                                    .Where(p => p.Knjiga == knjiga1);
                
                var knjigaBiblioteka1 = knjigaBibliotekaUpit.FirstOrDefault();

                if (knjigaBiblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{nazivKnjige}' ne postoji u biblioteci '{biblioteka1.Naziv}'."});
                }

                // knjiga postoji u biblioteci, kao i korisnik u biblioteci, treba samo da se ispita da li ima knjge na stanju
                if (knjigaBiblioteka1.Preostalo == 0)
                {
                    return BadRequest(new { Poruka = $"'{knjiga1.ZaPrikaz}' - nema na stanju!"});
                }

                //smanjiti knjigaBiblioteka.Presotalo za 1
                knjigaBiblioteka1.Preostalo--;
                Context.KnjigeBiblioteke.Update(knjigaBiblioteka1);

                Iznajmljivanje iznajmljivanje = new Iznajmljivanje();
                iznajmljivanje.Korisnik = korisnik2;
                iznajmljivanje.Biblioteka = biblioteka1;
                iznajmljivanje.Knjiga = knjiga1;
                //iznajmljivanje.KnjigaVracena = false;
                Context.Iznajmljivanja.Add(iznajmljivanje);

                await Context.SaveChangesAsync();

                return Ok(new { Poruka = $"Korisnik '{korisnik2.ZaPrikaz}' je iznajmio knjigu '{knjiga1.ZaPrikaz}'."});

            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }                                  
        }

        [Route("VratiKnjigu")]
        [HttpDelete]
        public async Task<ActionResult> VratiKnjigu([FromQuery] string kontaktBiblioteke, [FromQuery] string nazivKorisnika, [FromQuery] string nazivKnjige, [FromQuery] int ocenaKorisnika)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKorisnika) || nazivKorisnika.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešan korisnik!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKnjige) || nazivKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešna knjiga!"});
            }

            if (ocenaKorisnika < 1 || ocenaKorisnika > 5)
            {
                return BadRequest(new { Poruka = "Ocena mora biti u opsegu od 1 do 5."});
            }

            try
            {
                var bibliotekaUpit = Context.Biblioteke.Where(p => p.Kontakt == kontaktBiblioteke);

                var biblioteka1 = bibliotekaUpit.FirstOrDefault();

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji."});
                }

                var korisnikUpit = Context.Korisnici.Where(p => p.Ime + " " + p.Prezime + " " + p.JMBG == nazivKorisnika);

                var korisnik1 = korisnikUpit.FirstOrDefault();

                if (korisnik1 is null)
                {
                    return BadRequest(new { Poruka = $"Korisnik '{nazivKorisnika}' ne postoji."});
                }

                var korisnik2 = korisnikUpit.Include(p => p.Biblioteka).FirstOrDefault();

                if (korisnik2.Biblioteka.Kontakt != kontaktBiblioteke)
                {
                    return BadRequest(new { Poruka = $"Korisnik '{korisnik2.ZaPrikaz}' ne pripada biblioteci '{biblioteka1.Naziv}', vec '{korisnik2.Biblioteka.Naziv}'."});
                }

                var knjigaUpit = Context.Knjige.Where(p => p.Autor + " - " + p.Naslov == nazivKnjige);
                var knjiga1 = knjigaUpit.FirstOrDefault();

                if (knjiga1 is null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{nazivKnjige}' ne postoji!"});
                }

                var knjigaBibliotekaUpit = Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                                    .Where(p => p.Biblioteka == biblioteka1)
                                                                    .Include(p => p.Knjiga)
                                                                    .Where(p => p.Knjiga == knjiga1);
                
                var knjigaBiblioteka1 = knjigaBibliotekaUpit.FirstOrDefault();

                if (knjigaBiblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{nazivKnjige}' ne postoji u biblioteci '{biblioteka1.Naziv}'."});
                }

                // knjiga postoji u biblioteci, kao i korisnik u biblioteci, treba samo da se ispita da li je moguce vratiti
                if (knjigaBiblioteka1.Preostalo >= knjigaBiblioteka1.Kolicina )
                {
                    return BadRequest(new { Poruka = $"Nemoguce vratiti knjigu '{knjiga1.ZaPrikaz}'!"});
                }

                var iznajmljivanje = Context.Iznajmljivanja.Include(p => p.Biblioteka)
                                                            .Where(p => p.Biblioteka == biblioteka1)
                                                            .Include(p => p.Korisnik)
                                                            .Where(p => p.Korisnik == korisnik2)
                                                            .Include(p => p.Knjiga)
                                                            .Where(p => p.Knjiga == knjiga1)
                                                            // .Where(p => p.KnjigaVracena == false)
                                                            .FirstOrDefault();

                if (iznajmljivanje is null)
                {
                    return BadRequest(new { Poruka = $"Korisnik '{korisnik1.ZaPrikaz}' nije iznajmio knjigu '{knjiga1.ZaPrikaz}'."});
                }

                // povecati knjigaBiblioteka.Presotalo za 1
                knjigaBiblioteka1.Preostalo++;
                Context.KnjigeBiblioteke.Update(knjigaBiblioteka1);

                Context.Iznajmljivanja.Remove(iznajmljivanje);

                var stariBrojOcenjivanja = knjiga1.BrojOcenjivanja++;
                knjiga1.Ocena = (stariBrojOcenjivanja * knjiga1.Ocena + ocenaKorisnika) / knjiga1.BrojOcenjivanja;
                Context.Knjige.Update(knjiga1);

                await Context.SaveChangesAsync();

                return Ok(new { 
                    Ocena = Math.Round(knjiga1.Ocena, 2),
                    Poruka = $"Korisnik '{korisnik2.ZaPrikaz}' je vratio knjigu '{knjiga1.ZaPrikaz}' i dao ocenu '{ocenaKorisnika}'."
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }      
        }
    }
}