using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Data;
using ClinicApp.Models;
using System.Globalization;

namespace ClinicApp.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly ClinicAppContext _context;
        public int CurrentWeekNumber {  get; set; }

        public ShiftsController(ClinicAppContext context)
        {
            _context = context;
            //var currentDate = DateTime.Now;
            //CurrentWeekNumber = CultureInfo.CurrentCulture.Calendar
            //                                .GetWeekOfYear(currentDate, 
            //                                CalendarWeekRule.FirstFourDayWeek,DayOfWeek.Monday);
            //ViewData["currentWeek"] = CurrentWeekNumber;

        }

        // GET: Shifts
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentWeekNumber = Parameters.CurrentWeekNumber;
            var clinicAppContext = _context.Shift.Include(s => s.Doctor);
            return View(await clinicAppContext.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shift == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email");
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,StartTime,EndTime")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", shift.DoctorId);
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shift == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", shift.DoctorId);
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,StartTime,EndTime")] Shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", shift.DoctorId);
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shift == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shift == null)
            {
                return Problem("Entity set 'ClinicAppContext.Shift'  is null.");
            }
            var shift = await _context.Shift.FindAsync(id);
            if (shift != null)
            {
                _context.Shift.Remove(shift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
          return (_context.Shift?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult DecreaseNumberWeeks()
        {
            Parameters.CurrentWeekNumber--;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult IncreaseNumberWeeks() 
        { 
            Parameters.CurrentWeekNumber++;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CopyPreviosWeek()
        {
            DateTime startWeek, endWeek;
            GetWeekRange(out startWeek, out endWeek);

            var sourceWeekShifts = _context.Shift
                .Where(shift => shift.StartTime >= startWeek && shift.EndTime <= endWeek)
                .ToList();

            var targetWeekShifts = sourceWeekShifts.Select(shift =>
            {
                var adjustedStartTime = shift.StartTime.AddDays(7);
                var adjustedEndTime = shift.EndTime.AddDays(7);

                return new Shift
                {
                    StartTime = adjustedStartTime,
                    EndTime = adjustedEndTime,
                    Doctor = shift.Doctor,
                    DoctorId = shift.DoctorId
                };
            }).ToList();
            _context.Shift.AddRange(targetWeekShifts);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private static void GetWeekRange(out DateTime startWeek, out DateTime endWeek)
        {
            int year = DateTime.Now.Year;
            DateTime startYear = new DateTime(year, 1, 1);
            int weekNumber = Parameters.CurrentWeekNumber;
            startWeek = startYear.AddDays((weekNumber - 1) * 7 -
                                                    (int)startYear.DayOfWeek + (int)DayOfWeek.Monday);
            endWeek = startWeek.AddDays(6);
        }
    }
}
