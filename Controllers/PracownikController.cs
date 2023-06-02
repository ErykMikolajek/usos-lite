using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPracownik.Data;
using MvcPracownik.Models;

namespace lab10.Controllers
{
    public class PracownikController : Controller
    {
        private readonly MvcPracownikContext _context;

        public PracownikController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: Pracownik
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Logged in") == null)
                return View("Views/Auth/Login.cshtml");
            var prac = _context.Pracownik.Include(p => p.Zajecia).AsNoTracking();
            return View(await prac.ToListAsync());
        }

        // GET: Pracownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik = await _context.Pracownik
                .Include(p => p.Zajecia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }

        // GET: Pracownik/Create
        public IActionResult Create()
        {
            PopulateZajeciaDropDownList();
            return View();
        }

        // POST: Pracownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,DataZatrudnienia")] Pracownik pracownik,
            IFormCollection form)
        {
            string zajeciaValue = form["ZajeciaDropDown"];
            if (ModelState.IsValid)
            {
                Zajecia zajecia = null;
                if (zajeciaValue != "-1")
                {
                    var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                    if (ee.Count() > 0)
                        zajecia = ee.First();
                }
                pracownik.Zajecia = zajecia;

                _context.Add(pracownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pracownik);
        }

        // GET: Pracownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pracownik == null)
            {
                return NotFound();
            }


            var pracownik = _context.Pracownik.Where(p => p.Id == id)
                .Include(p => p.Zajecia).First();
            if (pracownik == null)
            {
                return NotFound();
            }
            if (pracownik.Zajecia != null)
            {
                PopulateZajeciaDropDownList(pracownik.Zajecia.Id_zajec);
            }
            else
            {
                PopulateZajeciaDropDownList();
            }

            return View(pracownik);
        }

        // POST: Pracownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,DataZatrudnienia")] Pracownik pracownik, IFormCollection form)
        {
            if (id != pracownik.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    string zajeciaValue = form["ZajeciaDropDown"];


                    Zajecia zajecia = null;
                    if (zajeciaValue != "-1")
                    {
                        var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                        if (ee.Count() > 0)
                            zajecia = ee.First();
                    }
                    pracownik.Zajecia = zajecia;

                    //Aby kontekst śledził zmiany w referowanych kolumnach etat oraz zespol
                    //należy "dostać" się do obiektu przez dbContext i dołączyć obiekty etat
                    //i zespol. Bez tego kolumny etat i zespół nie będą mogły być zmodyfikowane
                    //wartością NULL-ową, czyli nie będzie się dało usunąć powiązania. 
                    //Ustawienie na inną wartość niż NULL będzie działać przy "zwykłym"
                    // _context.Update(pracownik);
                    Pracownik pp = _context.Pracownik.Where(p => p.Id == id)
                    .Include(p => p.Zajecia)
                    .First();
                    pp.Zajecia = zajecia;
                    pp.Imie = pracownik.Imie;
                    pp.Nazwisko = pracownik.Nazwisko;
                    pp.DataZatrudnienia = pracownik.DataZatrudnienia;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracownikExists(pracownik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pracownik);
        }

        // GET: Pracownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik = _context.Pracownik.Where(p => p.Id == id)
                .Include(p => p.Zajecia).First();
            //var pracownik = await _context.Pracownicy
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }

        // POST: Pracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pracownik == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Pracownik'  is null.");
            }
            var pracownik = await _context.Pracownik.FindAsync(id);
            if (pracownik != null)
            {
                _context.Pracownik.Remove(pracownik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracownikExists(int id)
        {
            return (_context.Pracownik?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void PopulateZajeciaDropDownList(object selectedZajecia = null)
        {
            var wybraneZajecia = from z in _context.Zajecia
                                 orderby z.Nazwa
                                 select z;
            var res = wybraneZajecia.AsNoTracking();
            ViewBag.ZajeciaID = new SelectList(res, "Id_zajec", "Nazwa", selectedZajecia);
        }
    }
}

/* TODO:

brak pozostalych create edit w innych controlerach etc...

*/