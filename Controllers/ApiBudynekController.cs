using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcPracownik.Data;
using Microsoft.Data.Sqlite;
using MvcPracownik.Models;

namespace ControllerREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudynekController : ControllerBase
    {
        private readonly MvcPracownikContext _context;

        public BudynekController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: api/Budynek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budynek>>> GetBudynek()
        {
            if (_context.Budynek == null)
            {
                return NotFound();
            }
            return await _context.Budynek
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/Budynek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Budynek>> GetBudynek(int id)
        {
            if (_context.Budynek == null)
            {
                return NotFound();
            }
            var budynek = await _context.Budynek.FindAsync(id);

            if (budynek == null)
            {
                return NotFound();
            }

            return ItemToDTO(budynek);
        }

        // PUT: api/Budynek/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudynek(int id, Budynek budynek)
        {
            if (id != budynek.Id_budynku)
            {
                return BadRequest();
            }
            var informacja = await _context.Budynek.FindAsync(id);
            if (informacja == null)
            {
                return NotFound();
            }

            informacja.Nazwa = budynek.Nazwa;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InformacjaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Budynek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Budynek>> PostBudynek(Budynek budynek)
        {
            if (_context.Budynek == null)
            {
                return Problem("Entity set 'ContextDb.Informacja'  is null.");
            }

            var informacja = new Budynek
            {
                Nazwa = budynek.Nazwa,
            };
            _context.Budynek.Add(informacja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudynek", new { id = budynek.Id_budynku },
                ItemToDTO(informacja));
        }

        // DELETE: api/Budynek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudynek(int id)
        {
            if (_context.Budynek == null)
            {
                return NotFound();
            }
            var informacja = await _context.Budynek.FindAsync(id);
            if (informacja == null)
            {
                return NotFound();
            }

            _context.Budynek.Remove(informacja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InformacjaExists(int id)
        {
            return (_context.Budynek?.Any(e => e.Id_budynku == id)).GetValueOrDefault();
        }

        private static Budynek ItemToDTO(Budynek budynek) =>
            new Budynek
            {
                Id_budynku = budynek.Id_budynku,
                Nazwa = budynek.Nazwa
            };
    }
}