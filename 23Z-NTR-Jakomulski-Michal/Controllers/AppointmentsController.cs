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
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using Microsoft.AspNetCore.Identity;
using static ClinicApp.Models.Doctor;
using Microsoft.CodeAnalysis;

namespace ClinicApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ClinicAppContext _context;
        private readonly IConfiguration _configuration;
        public int CurrentWeekNumber { get; set; }

        private readonly UserManager<AppUser> _userManager;

        public AppointmentsController(ClinicAppContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: Appointments
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> Index()
        {
            PrepareViemVariables();
            var clinicAppContext = _context.Appointment.Include(s => s.Doctor);
            return View(await clinicAppContext.ToListAsync());
        }


        private void PrepareViemVariables()
        {
           
            if (TempData.Keys.Contains("CurrentWeek"))
                ViewBag.CurrentWeekNumber = TempData["CurrentWeek"] as int?;
            else
            {
                int weekNuber = CurrentWeekNumber = CultureInfo.CurrentCulture.Calendar
                                            .GetWeekOfYear(DateTime.Now,
                                            CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                ViewBag.CurrentWeekNumber = weekNuber;
                TempData["CurrentWeek"] = weekNuber;
            }
            TempData["CurrentWeek"] = ViewBag.CurrentWeekNumber;

            DateTime startWeek, endWeek;
            GetWeekRange(out startWeek, out endWeek);
            ViewBag.StartWeek = startWeek.ToString("yyyy-MM-dd");
            ViewBag.EndWeek = endWeek.ToString("yyyy-MM-dd");
            ViewBag.Directory = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

        }

        private async Task PrepareViewBag()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.patientId = user.PatientId;
            ViewBag.doctorId = user.DoctorId;
            //ViewData["Specialization"] = "All";

        }

        [Authorize(Roles = "Pat")]
        public async Task<IActionResult> AppointmentsListPatient(string? Specialization="All")
        {
            if (TempData.Keys.Contains("SpecializationNext"))
            {
                Specialization = TempData["SpecializationNext"] as string;
            }
            PrepareViemVariables();
            var clinicAppContext = _context.Appointment.Include(s => s.Doctor);
            await PrepareViewBag();
            List<Appointment> appointments = await clinicAppContext.ToListAsync();
            TempData["Specialization"] = Specialization;
            var user = await _userManager.GetUserAsync(User);
            appointments = appointments.Where(a => a.PatientId == null || a.PatientId == user.PatientId).ToList();
            return View(appointments.Where(a => 
            Enum.GetName(typeof(Doctor.Sepecializations), a.Doctor.Sepcialization) == Specialization || 
            Specialization == "All"));
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AppointmentsListDoctor()
        {
            PrepareViemVariables();
            var clinicAppContext = _context.Appointment.Include(s => s.Doctor);
            await PrepareViewBag();
            return View(await clinicAppContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var Appointment = await _context.Appointment
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Appointment == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            return View(Appointment);
        }

        // GET: Appointments/Create
        public IActionResult CreateShift()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,StartTime")] Appointment Appointment)
        {

            if (ModelState.IsValid)
            {
                _context.Add(Appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", Appointment.DoctorId);
            return View(Appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var Appointment = await _context.Appointment.FindAsync(id);
            if (Appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", Appointment.DoctorId);
            return View(Appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,StartTime")] Appointment Appointment)
        {
            if (id != Appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(Appointment.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email", Appointment.DoctorId);
            return View(Appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var Appointment = await _context.Appointment
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Appointment == null)
            {
                return NotFound();
            }

            return View(Appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'ClinicAppContext.Appointment'  is null.");
            }
            var Appointment = await _context.Appointment.FindAsync(id);
            if (Appointment != null)
            {
                _context.Appointment.Remove(Appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return (_context.Appointment?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult DecreaseNumberWeeks()
        {
            int? cw = TempData["CurrentWeek"] as int?;
            cw--;
            TempData["CurrentWeek"] = cw;

            return RedirectToAction("Index");
        }
        public IActionResult IncreaseNumberWeeks()
        {
            int? cw = TempData["CurrentWeek"] as int?;
            cw++;
            TempData["CurrentWeek"] = cw;

            return RedirectToAction("Index");
        }

        public IActionResult DecreaseNumberWeeksPatient()
        {
            //Parameters.CurrentWeekNumber--;
            TempData["SpecializationNext"] = TempData["Specialization"] as string;
            int? cw = TempData["CurrentWeek"] as int?;
            cw--;
            TempData["CurrentWeek"] = cw;

            return RedirectToAction("AppointmentsListPatient");
        }

        public IActionResult IncreaseNumberWeeksPatient()
        {
            //Parameters.CurrentWeekNumber++;
            TempData["SpecializationNext"] = TempData["Specialization"] as string;
            int? cw = TempData["CurrentWeek"] as int?;
            cw++;
            TempData["CurrentWeek"] = cw;

            return RedirectToAction("AppointmentsListPatient");
        }

        public IActionResult DecreaseNumberWeeksDoctor()
        {
            int? cw = TempData["CurrentWeek"] as int?;
            cw--;
            TempData["CurrentWeek"] = cw;


            return RedirectToAction("AppointmentsListDoctor");
        }
        public IActionResult IncreaseNumberWeeksDoctor()
        {
            int? cw = TempData["CurrentWeek"] as int?;
            cw++;
            TempData["CurrentWeek"] = cw;


            return RedirectToAction("AppointmentsListDoctor");
        }

        public IActionResult CopyPreviosWeek()
        {
            DateTime startWeek, endWeek;
            GetWeekRange(out startWeek, out endWeek);

            var sourceWeekAppointments = _context.Appointment
                .Where(Appointment => Appointment.StartTime >= startWeek && Appointment.StartTime <= endWeek)
                .ToList();

            var targetWeekAppointments = sourceWeekAppointments.Select(Appointment =>
            {
                var adjustedStartTime = Appointment.StartTime.AddDays(7);


                return new Appointment
                {
                    StartTime = adjustedStartTime,
                    Doctor = Appointment.Doctor,
                    DoctorId = Appointment.DoctorId
                };
            }).ToList();
            _context.Appointment.AddRange(targetWeekAppointments);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void GetWeekRange(out DateTime startWeek, out DateTime endWeek)
        {
            int year = DateTime.Now.Year;
            DateTime startYear = new DateTime(year, 1, 1);
            int weekNumber = (int)ViewBag.CurrentWeekNumber;
            startWeek = startYear.AddDays((weekNumber - 1) * 7 -
                                                    (int)startYear.DayOfWeek + (int)DayOfWeek.Monday);
            endWeek = startWeek.AddDays(6);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift([Bind("DoctorId,StartTime,EndTime")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                await CreateAppointments(shift);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Email");

            return View();
        }
        public async Task CreateAppointments(Shift shift)
        {
            try
            {
                int numberAppointemts = CreateNumberOfAppointments(shift.StartTime, shift.EndTime);
                DateTime appointemntStart = shift.StartTime;
                int doctorId = shift.DoctorId;

                for (int i = 0; i < numberAppointemts; i++)
                {
                    // TODO: How to manage properly null values
                    await Create(new Appointment
                    {
                        DoctorId = doctorId,
                        StartTime = appointemntStart
                    });
                    appointemntStart = appointemntStart.AddMinutes(15);
                }
            }
            catch (ArgumentException exception)
            {


            }
        }

        private int CreateNumberOfAppointments(DateTime start, DateTime end)
        {
            TimeSpan difference = end - start;

            int visitsCuantity = (int)(difference.TotalMinutes / 15);
            int AppointmentDivided = (int)(difference.TotalMinutes % 15);


            if (difference < TimeSpan.Zero)
                throw new ArgumentException("The start date is grater then the end date");
            else if (AppointmentDivided != 0)
            {
                double minutesBefore = AppointmentDivided * 15;
                double minutesAfter = (AppointmentDivided + 1) * 15;

                string appointementBefore = start.AddMinutes(minutesBefore).ToString("HH:mm");
                string appointementAfter = start.AddMinutes(minutesAfter).ToString("HH:mm");

                // TODO: user define exception so I can return some arguemnts in the exception
                throw new ArgumentException($"The shift can't be divided to appointments chose: " +
                                            $"{appointementBefore} or {appointementAfter}");
            }

            return visitsCuantity;

        }

        public async Task<IActionResult> AddDescription(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }
            var appointemnt = await _context.Appointment.FindAsync(id);
            _context.Dispose(); // TODO: Check if it's necessary
            if (appointemnt == null)
            {
                return NotFound();
            }
            if (appointemnt.Description == null)
            {
                appointemnt.Description = "";
            }
            return View(appointemnt);
        }

        [HttpPost]
        public async Task<IActionResult> AddDescription(int id, [Bind("Id,DoctorId,StartTime,Description")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AppointmentsListDoctor));
            }
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            AppUser? user = await _userManager.GetUserAsync(User);

            int? patientId = user.PatientId;
            appointment.PatientId = patientId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AppointmentsListPatient));
        }

        [HttpPost]
        public async Task<IActionResult> SignOut(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }


            appointment.PatientId = null;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AppointmentsListPatient));
        }

        public IActionResult ChoseSpecialization(string specialization)
        {
            
            ViewBag.specialization = specialization;
            return RedirectToAction(nameof(AppointmentsListPatient));
        }



        [HttpPost]
        public async Task<IActionResult> GeneratePdf([Bind("StartTime, EndTime")] PdfScope pdfScope)
        {
            var start = pdfScope.StartTime;
            var end = pdfScope.EndTime;
            var clinicAppDoctors = _context.Doctor.Include(s => s.Appointments);
            List<Doctor> doctors = await clinicAppDoctors.ToListAsync();
            PdfConverter pdf = new PdfConverter();
            pdf.GenerateRaport(start, end, doctors); // TODO: Why it can't be static?

            return RedirectToAction(nameof(Index));
        }



        public IActionResult GeneratePdf()
        {
            return View();
        }

        public IActionResult DownloadReport()
        {
         
            string? projectDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            PhysicalFileResult file = PhysicalFile(projectDir + "\\raport.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf, "raport.pdf");
            if (System.IO.File.Exists(projectDir + "\\raport.pdf"))
            {
                
                return file;
            }
            else
            {
                ViewBag.FileNotFound = true;
                return RedirectToAction(nameof(Index));
            }
           
        }

 
    }
}
