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
        public async Task<ActionResult> PreuzmiKnjigeIzBiblioteke([FromQuery] int idBiblioteke)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest(new { Poruka = "Morate izabrati biblioteku!"});
            }

            try
            {
                var biblioteka1 = DaLiPostojiBiblioteka(idBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
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
                            ID = p.ID,
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

        [HttpGet]
        [Route("PretragaKnjige")]
        public async Task<ActionResult> PretragaKnjige([FromQuery] int idBiblioteke,[FromQuery] string autor,[FromQuery] string naslov)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest("Biblioteka ne postoji!");
            }

            if (string.IsNullOrWhiteSpace(autor) && string.IsNullOrWhiteSpace(naslov))
            {
                return BadRequest( new { Poruka = "Morate uneti parametre pretrage!" });
            }

            try
            {
                var biblioteka = await Context.Biblioteke.FindAsync(idBiblioteke);

                if (biblioteka is null)
                {
                    return BadRequest("Biblioteka ne postoji!");
                }

                if (string.IsNullOrWhiteSpace(autor))
                {
                    var filter = await Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                        .Where(p => p.Biblioteka == biblioteka)
                                                        .Include(p => p.Knjiga)
                                                        .ThenInclude(p => p.Biblioteke)
                                                        .Where(p => p.Knjiga.Naslov.Contains(naslov))
                                                        .Select(p => p.Knjiga)
                                                        .ToListAsync();
                                                        
                    if (filter.Count <= 0)
                    {
                        return BadRequest("Nema podudaranja!");
                    }
                    
                    return Ok(
                            filter.Select(p =>
                                new
                                {   
                                    ID = p.ID,
                                    Autor = p.Autor,
                                    Naslov = p.Naslov,
                                    Prikaz = p.ZaPrikaz,
                                    Ocena = p.Ocena >= 1 ? Math.Round(p.Ocena, 2).ToString() : "UNK",
                                    Kolicina = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Kolicina).FirstOrDefault(),
                                    Preostalo = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Preostalo).FirstOrDefault()
                                }).ToList()
                        );
                }
                else if (string.IsNullOrWhiteSpace(naslov))
                {
                    var filter = await Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                        .Where(p => p.Biblioteka == biblioteka)
                                                        .Include(p => p.Knjiga)
                                                        .ThenInclude(p => p.Biblioteke)
                                                        .Where(p => p.Knjiga.Autor.Contains(autor))
                                                        .Select(p => p.Knjiga)
                                                        .ToListAsync();
                    if (filter.Count <= 0)
                    {
                        return BadRequest("Nema podudaranja!");
                    }
                    
                    return Ok(
                            filter.Select(p =>
                                new
                                {   
                                    ID = p.ID,
                                    Autor = p.Autor,
                                    Naslov = p.Naslov,
                                    Prikaz = p.ZaPrikaz,
                                    Ocena = p.Ocena >= 1 ? Math.Round(p.Ocena, 2).ToString() : "UNK",
                                    Kolicina = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Kolicina).FirstOrDefault(),
                                    Preostalo = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Preostalo).FirstOrDefault()
                                }).ToList()
                        );
                }
                else
                {
                    var filter = await Context.KnjigeBiblioteke.Include(p => p.Biblioteka)
                                                        .Where(p => p.Biblioteka == biblioteka)
                                                        .Include(p => p.Knjiga)
                                                        .ThenInclude(p => p.Biblioteke)
                                                        .Where(p => p.Knjiga.Autor.Contains(autor) && p.Knjiga.Naslov.Contains(naslov))
                                                        .Select(p => p.Knjiga)
                                                        .ToListAsync();

                    if (filter.Count <= 0)
                    {
                        return BadRequest("Nema podudaranja!");
                    }

                    return Ok(
                            filter.Select(p =>
                                new
                                {   
                                    ID = p.ID,
                                    Autor = p.Autor,
                                    Naslov = p.Naslov,
                                    Prikaz = p.ZaPrikaz,
                                    Ocena = p.Ocena >= 1 ? Math.Round(p.Ocena, 2).ToString() : "UNK",
                                    Kolicina = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Kolicina).FirstOrDefault(),
                                    Preostalo = p.Biblioteke.Where(q => q.Biblioteka == biblioteka).Select(q => q.Preostalo).FirstOrDefault()
                                }).ToList()
                        );
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message });
            }
        }

        [Route("DodajKnjiguUBiblioteku")]
        [HttpPost]
        public async Task<ActionResult> DodajKnjiguUBiblioteku([FromQuery] int idBiblioteke, [FromQuery] string autorKnjige, [FromQuery] string naslovKnjige, [FromQuery] int kolicinaKnjige)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest(new { Poruka = "Morate izabrati biblioteku."});
            }

            if (string.IsNullOrWhiteSpace(naslovKnjige) || naslovKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogre??an naslov knjige!"});
            }

            if (string.IsNullOrWhiteSpace(autorKnjige) || autorKnjige.Length > 50)
            {
                return BadRequest(new { Poruka = "Pogre??an autor knjige!"});
            }

            if (kolicinaKnjige < 1 || kolicinaKnjige > 100)
            {
                return BadRequest(new { Poruka = "Nevalidna koli??ina!"});
            }

            try
            {
                var biblioteka1 = DaLiPostojiBiblioteka(idBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
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
                
                return Ok(new { 
                    ID = knjiga1.ID,
                    Ocena = knjiga1.Ocena >= 1 ? Math.Round(knjiga1.Ocena, 2).ToString() : "UNK",
                    Poruka = $"Knjiga '{knjiga1.ZaPrikaz}' je uspe??no dodata u biblioteku '{biblioteka1.Naziv}'."
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmeniKnjiguUBiblioteci")]
        [HttpPut]
        public async Task<ActionResult> IzmeniKnjiguUBiblioteci([FromQuery] int idBiblioteke, [FromQuery] int idKnjige, [FromQuery] int novaKolicinaKnjige)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
            }

            if (idKnjige <= 0)
            {
                return BadRequest(new { Poruka = "Knjiga ne postoji!"});
            }

            if (novaKolicinaKnjige < 1 || novaKolicinaKnjige > 100)
            {
                return BadRequest(new { Poruka = "Nevalidna koli??ina!"});
            }

            try
            {
                var biblioteka1 = DaLiPostojiBiblioteka(idBiblioteke);

                if (biblioteka1 is null)
                {
                    return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
                }

                var knjiga1 = DaLiPostojiKnjiga(idKnjige);

                if (knjiga1 is null) // ispitujemo da li knjiga postoji
                {
                    return BadRequest(new { Poruka = "Knjiga ne postoji."});
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
                    return BadRequest(new { Poruka = $"Nova koli??ina ne mo??e biti {novaKolicinaKnjige}, jer je izdato {knjigaPostoji.Kolicina - knjigaPostoji.Preostalo} knjiga."});
                }
                
                var staraKolicina = knjigaPostoji.Kolicina;
                knjigaPostoji.Kolicina = novaKolicinaKnjige;
                knjigaPostoji.Preostalo += novaKolicinaKnjige - staraKolicina;

                Context.KnjigeBiblioteke.Update(knjigaPostoji);

                await Context.SaveChangesAsync();

                return Ok(new { Poruka = $"'{knjiga1.ZaPrikaz}', koli??ina: {knjigaPostoji.Kolicina}."});
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [Route("UkloniKnjiguIzBiblioteke")]
        [HttpDelete]
        public async Task<ActionResult> UkloniKnjiguIzBiblioteke([FromQuery] int idBiblioteke, [FromQuery] int idKnjige)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
            }

            if (idKnjige <= 0)
            {
                return BadRequest(new { Poruka = "Knjiga ne postoji!"});
            }

            // ispitati da li postoji zeljena knjiga i biblioteka uopste, zatim da li knjiga pripada biblioteci
            // ako je nekom izdata knjiga ne moze se obrisati
            // zatim obrisati taj spoj i sacuvati u bazi
            // nakon toga ispitati da li ta knjiga pripada jos nekoj biblioteci u bazi
            // ako ne pripada, obrisati knjigu skroz iz baze, a ako pripada, ostaviti je

            try
            {
                var biblioteka = DaLiPostojiBiblioteka(idBiblioteke);
                if (biblioteka is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka ne postoji!"});
                }

                var knjiga = DaLiPostojiKnjiga(idKnjige);
                if (knjiga == null) // ispitujemo da li knjiga postoji
                {
                    return BadRequest(new { Poruka = $"Knjiga ne postoji."});
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
                    return BadRequest(new { Poruka = $"Knjiga '{knjiga.ZaPrikaz}' se ne mo??e obrisati jer je iznajmljena(Broj iznajmljivanja: {knjigaBiblioteka.Kolicina - knjigaBiblioteka.Preostalo})!"});
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

                return Ok(new { Poruka = $"Knjiga uspe??no uklonjena iz biblioteke '{biblioteka.Naziv}'." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Poruka = e.Message});
            }
        }

        [Route("Top5KnjigaIzBiblioteke")]
        [HttpGet]
        public async Task<ActionResult> Top5KnjigaIzBiblioteke([FromQuery] int idBiblioteke)
        {
            if (idBiblioteke <= 0)
            {
                return BadRequest(new { Poruka = "Biblioteka ne postoji!"});
            }

            try
            {
                var biblioteka = DaLiPostojiBiblioteka(idBiblioteke);
                if (biblioteka is null)
                {
                    return BadRequest(new { Poruka = $"Biblioteka ne postoji!" });
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
        private Biblioteka DaLiPostojiBiblioteka(int idBiblioteke)
        {
            return Context.Biblioteke.Find(idBiblioteke);
        }

        [HttpGet]
        private Knjiga DaLiPostojiKnjiga(int idKnjige)
        {
            return Context.Knjige.Find(idKnjige);
        }

        [HttpGet]
        private Knjiga DaLiPostojiKnjiga(string autorKnjige, string naslovKnjige)
        {
            return Context.Knjige.Where(p => p.Autor == autorKnjige && p.Naslov == naslovKnjige).FirstOrDefault();
        }
    }
}