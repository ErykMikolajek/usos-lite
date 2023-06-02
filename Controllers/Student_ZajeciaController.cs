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
    public class Student_ZajeciaController : Controller
    {
        private readonly MvcPracownikContext _context;

        public Student_ZajeciaController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: Student_Zajecia
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Logged in") == null)
                return View("Views/Auth/Login.cshtml");
            var Student_Zajecia = _context.Student_Zajecia
                .Include(p => p.student)
                .Include(p => p.zajecia)
                .AsNoTracking();
            return View(await Student_Zajecia.ToListAsync());
        }

        // GET: Student_Zajecia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = await _context.Student_Zajecia
                .FirstOrDefaultAsync(m => m.Student_Zajecia_ID == id);
            if (student_Zajecia == null)
            {
                return NotFound();
            }

            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Create
        public IActionResult Create()
        {
            PopulateStudentDropDownList();
            PopulateZajeciaDropDownList();
            return View();
        }

        // POST: Student_Zajecia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Student_Zajecia_ID")] Student_Zajecia student_Zajecia,
            IFormCollection form)
        {
            string zajeciaValue = form["ZajeciaDropDown"];
            string studentValue = form["StudentDropDown"];

            if (ModelState.IsValid)
            {
                Zajecia zajecia = null;
                if (zajeciaValue != "-1")
                {
                    var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                    if (ee.Count() > 0)
                        zajecia = ee.First();
                }
                student_Zajecia.zajecia = zajecia;

                Student student = null;
                if (studentValue != "-1")
                {
                    var ee = _context.Student.Where(e => e.Id == int.Parse(studentValue));
                    if (ee.Count() > 0)
                        student = ee.First();
                }
                student_Zajecia.student = student;

                _context.Add(student_Zajecia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = await _context.Student_Zajecia.FindAsync(id);
            if (student_Zajecia == null)
            {
                return NotFound();
            }

            if (student_Zajecia.zajecia != null)
            {
                PopulateZajeciaDropDownList(student_Zajecia.zajecia.Id_zajec);
            }
            else
            {
                PopulateZajeciaDropDownList();
            }
            if (student_Zajecia.student != null)
            {
                PopulateStudentDropDownList(student_Zajecia.student.Id);
            }
            else
            {
                PopulateStudentDropDownList();
            }


            return View(student_Zajecia);
        }

        // POST: Student_Zajecia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Student_Zajecia_ID")] Student_Zajecia student_Zajecia,
            IFormCollection form)
        {
            if (id != student_Zajecia.Student_Zajecia_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string zajeciaValue = form["ZajeciaDropDown"];

                    string studentValue = form["StudentDropDown"];

                    Student student = null;
                    if (zajeciaValue != "-1")
                    {
                        var ee = _context.Student.Where(e => e.Id == int.Parse(studentValue));
                        if (ee.Count() > 0)
                            student = ee.First();
                    }

                    Zajecia zajecia = null;
                    if (zajeciaValue != "-1")
                    {
                        var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                        if (ee.Count() > 0)
                            zajecia = ee.First();
                    }
                    student_Zajecia.zajecia = zajecia;
                    student_Zajecia.student = student;

                    //Aby kontekst śledził zmiany w referowanych kolumnach etat oraz zespol
                    //należy "dostać" się do obiektu przez dbContext i dołączyć obiekty etat
                    //i zespol. Bez tego kolumny etat i zespół nie będą mogły być zmodyfikowane
                    //wartością NULL-ową, czyli nie będzie się dało usunąć powiązania. 
                    //Ustawienie na inną wartość niż NULL będzie działać przy "zwykłym"
                    // _context.Update(pracownik);
                    Student_Zajecia pp = _context.Student_Zajecia.Where(p => p.Student_Zajecia_ID == id)
                    .Include(p => p.zajecia)
                    .Include(p => p.student)
                    .First();
                    pp.zajecia = student_Zajecia.zajecia;
                    pp.student = student_Zajecia.student;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Student_ZajeciaExists(student_Zajecia.Student_Zajecia_ID))
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
            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = _context.Student_Zajecia.Where(p => p.Student_Zajecia_ID == id)
                .Include(p => p.zajecia)
                .Include(p => p.student)
                .First();
            if (student_Zajecia == null)
            {
                return NotFound();
            }

            return View(student_Zajecia);
        }

        // POST: Student_Zajecia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student_Zajecia == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Student_Zajecia'  is null.");
            }
            var student_Zajecia = await _context.Student_Zajecia.FindAsync(id);
            if (student_Zajecia != null)
            {
                _context.Student_Zajecia.Remove(student_Zajecia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Student_ZajeciaExists(int id)
        {
            return (_context.Student_Zajecia?.Any(e => e.Student_Zajecia_ID == id)).GetValueOrDefault();
        }

        private void PopulateZajeciaDropDownList(object selectedZajecia = null)
        {
            var wybraneZajecia = from z in _context.Zajecia
                                 orderby z.Nazwa
                                 select z;
            var res = wybraneZajecia.AsNoTracking();
            ViewBag.ZajeciaID = new SelectList(res, "Id_zajec", "Nazwa", selectedZajecia);
        }

        private void PopulateStudentDropDownList(object selectedStudent = null)
        {
            var wybranyStudent = from z in _context.Student
                                 orderby z.Nazwisko
                                 select z;
            var res = wybranyStudent.AsNoTracking();

            var selectSurnamesAndNames = res.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(), // Assuming "Id" is the property representing the student ID
                Text = s.Nazwisko + " " + s.Imie // Modify this line to include additional fields if needed
            });


            ViewBag.StudentID = new SelectList(selectSurnamesAndNames, "Value", "Text", selectedStudent);
        }
    }
}
