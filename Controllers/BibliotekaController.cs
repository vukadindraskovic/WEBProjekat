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
    public class BibliotekaController : ControllerBase
    {
        public BibliotekaContext Context { get; set; }

        public BibliotekaController(BibliotekaContext context)
        {
            Context = context;
        }

        [Route("PreuzmiDveBiblioteke")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiDveBiblioteke()
        {
            try
            {
                var biblioteke = await Context.Biblioteke.Take(2).ToListAsync();
                return Ok(
                    biblioteke.Select(p => 
                        new
                        {
                            Naziv = p.Naziv,
                            Adresa = p.Adresa,
                            Kontakt = p.Kontakt
                        })
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}