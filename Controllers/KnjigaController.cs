using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;

namespace WEBProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KnjigaController : ControllerBase
    {
        public BibliotekaContext Context { get; set; }

        public KnjigaController(BibliotekaContext context)
        {
            Context = context;
        }

        [Route("PreuzmiKnjigeIzBiblioteke")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiKnjigeIzBiblioteke([FromQuery] string kontaktBiblioteke)
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
                var biblioteka1 = DaLiPostojiBiblioteka(kontaktBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var knjigaUpit = Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                .Where(p => p.Biblioteka == biblioteka1)
                                                .Include(p => p.Knjiga)
                                                .ThenInclude(p => p.Biblioteke)
                                                .Select(p => p.Knjiga);
                                                
                var knjige = await knjigaUpit.ToListAsync();
                return Ok(
                    knjige.Select(p =>
                        new
                        {   
                            Autor = p.Autor,
                            Naslov = p.Naslov,
                            Prikaz = p.ZaPrikaz,
                            Ocena = p.Ocena >= 1 ? Math.Round(p.Ocena, 2).ToString() : "UNK",
                            Kolicina = p.Biblioteke.Where(q => q.Biblioteka == biblioteka1).Select(q => q.Kolicina).FirstOrDefault(),
                            Preostalo = p.Biblioteke.Where(q => q.Biblioteka == biblioteka1).Select(q => q.Preostalo).FirstOrDefault()
                        }).ToList()
                );
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [Route("DodajKnjiguUBiblioteku")]
        [HttpPost]
        public async Task<ActionResult> DodajKnjiguUBiblioteku([FromQuery] string kontaktBiblioteke, [FromQuery] string autorKnjige, [FromQuery] string naslovKnjige, [FromQuery] int kolicinaKnjige)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (string.IsNullOrWhiteSpace(naslovKnjige) || naslovKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešan naslov knjige!"});
            }

            if (string.IsNullOrWhiteSpace(autorKnjige) || autorKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogrešan autor knjige!"});
            }

            if (kolicinaKnjige < 1 || kolicinaKnjige > 100)
            {
                return BadRequest(new { Poruka = "Nevalidna količina!"});
            }

            try
            {
                var biblioteka1 = DaLiPostojiBiblioteka(kontaktBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var knjiga1 = DaLiPostojiKnjiga(autorKnjige, naslovKnjige);
                Knjiga knjigaZaUnos = null;

                if (knjiga1 != null) // ispitujemo da li knjiga vec postoji u biblioteci
                {
                    var knjigaPostoji = Context.KnjigeBiblioteke.Include(p => p.Knjiga)
                                            .Where(p => p.Knjiga == knjiga1)
                                            .Include(p => p.Biblioteka)
                                            .Where(p => p.Biblioteka == biblioteka1)
                                            .FirstOrDefault();
                    if (knjigaPostoji != null)
                    {
                        return BadRequest(new { Poruka = $"Knjiga '{knjiga1.ZaPrikaz}' vec postoji u ovoj biblioteci!"});
                    } 
                }
                else // knjiga ne postoji, kreira se
                {
                    knjigaZaUnos = new Knjiga();
                    knjigaZaUnos.Naslov = naslovKnjige;
                    knjigaZaUnos.Autor = autorKnjige;
                    knjigaZaUnos.Ocena = 0;
                    knjigaZaUnos.BrojOcenjivanja = 0;

                    Context.Knjige.Add(knjigaZaUnos);
                }
                // ovde se knjiga dodaje biblioteci
                var knjigaBiblioteka = new KnjigaBiblioteka();
                knjigaBiblioteka.Biblioteka = biblioteka1;

                if (knjigaZaUnos is null)
                    knjigaBiblioteka.Knjiga = knjiga1;
                else
                    knjigaBiblioteka.Knjiga = knjigaZaUnos;
                    
                knjigaBiblioteka.Kolicina = kolicinaKnjige;
                knjigaBiblioteka.Preostalo = kolicinaKnjige;

                Context.KnjigeBiblioteke.Add(knjigaBiblioteka);
                await Context.SaveChangesAsync();

                if (knjigaZaUnos != null)
                    knjiga1 = knjigaZaUnos;

                return Ok(new { Poruka = $"Knjiga '{knjiga1.ZaPrikaz}' je uspešno dodata u biblioteku '{biblioteka1.Naziv}'."});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmeniKnjiguUBiblioteci")]
        [HttpPut]
        public async Task<ActionResult> IzmeniKnjiguUBiblioteci([FromQuery] string kontaktBiblioteke, [FromQuery] string nazivKnjige, [FromQuery] int novaKolicinaKnjige)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (novaKolicinaKnjige < 1 || novaKolicinaKnjige > 100)
            {
                return BadRequest(new { Poruka = "Nevalidna količina!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKnjige) || nazivKnjige.Length > 103)
            {
                return BadRequest(new { Poruka = "Pogrešan stari naziv knjige!"});
            }

            try
            {
                var biblioteka1 = DaLiPostojiBiblioteka(kontaktBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var knjiga1 = DaLiPostojiKnjiga(nazivKnjige);

                if (knjiga1 is null) // ispitujemo da li knjiga postoji
                {
                    return BadRequest(new { Poruka = $"Knjiga {nazivKnjige} ne postoji."});
                }
                
                var knjigaPostoji = Context.KnjigeBiblioteke.Include(p => p.Knjiga)
                                            .Where(p => p.Knjiga == knjiga1)
                                            .Include(p => p.Biblioteka)
                                            .Where(p => p.Biblioteka == biblioteka1)
                                            .FirstOrDefault();
                if (knjigaPostoji is null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{knjiga1.ZaPrikaz}' ne postoji u biblioteci '{biblioteka1.Naziv}'."});
                }

                if (knjigaPostoji.Kolicina - knjigaPostoji.Preostalo > novaKolicinaKnjige)
                {
                    return BadRequest(new { Poruka = $"Nova količina ne može biti {novaKolicinaKnjige}, jer je izdato {knjigaPostoji.Kolicina - knjigaPostoji.Preostalo} knjiga."});
                }
                
                var staraKolicina = knjigaPostoji.Kolicina;
                knjigaPostoji.Kolicina = novaKolicinaKnjige;
                knjigaPostoji.Preostalo += novaKolicinaKnjige - staraKolicina;

                Context.KnjigeBiblioteke.Update(knjigaPostoji);

                await Context.SaveChangesAsync();

                return Ok(new { Poruka = $"'{knjiga1.ZaPrikaz}', količina: {knjigaPostoji.Kolicina}."});
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [Route("UkloniKnjiguIzBiblioteke")]
        [HttpDelete]
        public async Task<ActionResult> UkloniKnjiguIzBiblioteke([FromQuery] string kontaktBiblioteke, [FromQuery] string nazivKnjige)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!"});
            }

            if (string.IsNullOrWhiteSpace(nazivKnjige) || nazivKnjige.Length > 103)
            {
                return BadRequest(new { Poruka = "Pogrešan naziv knjige!"});
            }

            // ispitati da li postoji zeljena knjiga i biblioteka uopste, zatim da li knjiga pripada biblioteci
            // ako je nekom izdata knjiga ne moze se obrisati
            // zatim obrisati taj spoj i sacuvati u bazi
            // nakon toga ispitati da li ta knjiga pripada jos nekoj biblioteci u bazi
            // ako ne pripada, obrisati knjigu skroz iz baze, a ako pripada, ostaviti je

            try
            {
                var biblioteka = DaLiPostojiBiblioteka(kontaktBiblioteke);
                if (biblioteka is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!"});
                }

                var knjiga = DaLiPostojiKnjiga(nazivKnjige);
                if (knjiga == null) // ispitujemo da li knjiga postoji
                {
                    return BadRequest(new { Poruka = $"Knjiga '{nazivKnjige}' ne postoji."});
                }

                var knjigaBiblioteka = Context.KnjigeBiblioteke.Include(p => p.Knjiga)
                                            .Where(p => p.Knjiga == knjiga)
                                            .Include(p => p.Biblioteka)
                                            .Where(p => p.Biblioteka == biblioteka)
                                            .FirstOrDefault();
                if (knjigaBiblioteka == null)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{knjiga.ZaPrikaz}' ne postoji u biblioteci '{biblioteka.Naziv}'."});
                }

                if (knjigaBiblioteka.Preostalo != knjigaBiblioteka.Kolicina)
                {
                    return BadRequest(new { Poruka = $"Knjiga '{knjiga.ZaPrikaz}' se ne može obrisati jer je iznajmljena(Broj iznajmljivanja: {knjigaBiblioteka.Kolicina - knjigaBiblioteka.Preostalo})!"});
                }

                Context.KnjigeBiblioteke.Remove(knjigaBiblioteka);
                await Context.SaveChangesAsync();

                var knjigaPostoji = await Context.KnjigeBiblioteke.Include(p => p.Knjiga)
                                                            .Where(p => p.Knjiga == knjiga)
                                                            .ToListAsync();

                if (knjigaPostoji != null)
                {
                    Context.Knjige.Remove(knjiga);
                    await Context.SaveChangesAsync();
                }

                return Ok(new { Poruka = $"Knjiga uspešno uklonjena iz biblioteke '{biblioteka.Naziv}'." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [Route("Top5KnjigaIzBiblioteke")]
        [HttpGet]
        public async Task<ActionResult> Top5KnjigaIzBiblioteke([FromQuery] string kontaktBiblioteke)
        {
            if (string.IsNullOrWhiteSpace(kontaktBiblioteke) || kontaktBiblioteke.Length < 10 || kontaktBiblioteke.Length > 11)
            {
                return BadRequest(new { Poruka = "Pogrešan kontakt biblioteke!" });
            }

            try
            {
                var biblioteka = DaLiPostojiBiblioteka(kontaktBiblioteke);
                if (biblioteka is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka sa kontakt brojem {kontaktBiblioteke} ne postoji!" });
                }

                var sveKnjige = await Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                    .Where(p => p.Biblioteka == biblioteka)
                                                    .Include(p => p.Knjiga)
                                                    .Select(p => p.Knjiga)
                                                    .Where(p => p.Ocena >= 1)
                                                    .OrderByDescending(p => p.Ocena)
                                                    .ToListAsync();

                // var brojKnjiga = sveKnjige.Count() > 5 ? 5 : sveKnjige.Count();

                sveKnjige = sveKnjige.GetRange(0, sveKnjige.Count() > 5 ? 5 : sveKnjige.Count());
                return Ok(
                    sveKnjige.Select(p =>
                        new
                        {
                            Prikaz = p.ZaPrikaz,
                            Ocena = Math.Round(p.Ocena, 2)
                        }
                    )
                );
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [HttpGet]
        private Biblioteka DaLiPostojiBiblioteka(string kontaktBiblioteke)
        {
            return Context.Biblioteke.Where(p => p.Kontakt == kontaktBiblioteke).FirstOrDefault();
        }

        [HttpGet]
        private Knjiga DaLiPostojiKnjiga(string nazivKnjige)
        {
            return Context.Knjige.Where(p => p.Autor + " - " +  p.Naslov == nazivKnjige).FirstOrDefault();
        }

        [HttpGet]
        private Knjiga DaLiPostojiKnjiga(string autorKnjige, string naslovKnjige)
        {
            return Context.Knjige.Where(p => p.Autor == autorKnjige && p.Naslov == naslovKnjige).FirstOrDefault();
        }

        [Route("PreuzmiOcenuKnjige")]
        [HttpGet]
        public ActionResult PreuzmiOcenuKnjige([FromQuery] string nazivKnjige)
        {
            if (string.IsNullOrWhiteSpace(nazivKnjige) || nazivKnjige.Length > 103)
            {
                return BadRequest( new { Poruka = "Pogrešan naziv knjige!"});
            }

            try
            {
                var knjiga = DaLiPostojiKnjiga(nazivKnjige);
                if (knjiga == null) // ispitujemo da li knjiga postoji
                {
                    return BadRequest($"Knjiga '{nazivKnjige}' ne postoji.");
                }

                return Ok(
                    new
                    {
                        Ocena = Math.Round(knjiga.Ocena, 2).ToString()
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}