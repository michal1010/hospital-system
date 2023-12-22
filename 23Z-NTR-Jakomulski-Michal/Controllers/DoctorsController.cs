using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using ClinicApp.Data;
using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ClinicAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<AppUser> _roleManager;

        public DoctorsController(ClinicAppContext context, 
                                 UserManager<AppUser> userManager)
        {
            _context = context;
            //_roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return _context.Doctor != null ?
                        View(await _context.Doctor.ToListAsync()) :
                        Problem("Entity set 'ClinicAppContext.Doctor'  is null.");
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,Sepcialization,Email,Password")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                var user = UserFromDoctorCreate(doctor);
                var result = await _userManager.CreateAsync(user, doctor.Password);
                await _userManager.AddToRoleAsync(user, Constants.Roles.Doctor.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        private AppUser UserFromDoctorCreate(Doctor doctor)
        {
            AppUser au = Activator.CreateInstance<AppUser>();

            au.UserName = doctor.Email;
            au.Name = doctor.Name;
            au.Surname = doctor.Surname;
            au.Email = doctor.Email;
            au.EmailConfirmed = true;
            au.PhoneNumberConfirmed = true;
            au.Doctor = doctor;
            return au;
        }

        private AppUser UserFromDoctorUpdate(Doctor doctor, AppUser user)
        {

            user.UserName = doctor.Email;
            user.Name = doctor.Name;
            user.Surname = doctor.Surname;
            user.Email = doctor.Email;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.Doctor = doctor;
            return user;
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                           .AsNoTracking()
                           .FirstOrDefaultAsync(m => m.Id == id);
            _context.Dispose();
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, byte[] rowVersion)
        {


            var doctorToUpdate = await _context.Doctor.FirstOrDefaultAsync(d => d.Id == id);

            if (doctorToUpdate == null)
            {
                Doctor deletedDoctor = new Doctor();
                await TryUpdateModelAsync(deletedDoctor);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                return View(deletedDoctor);
            }

            _context.Entry(doctorToUpdate).Property("RowVersion").OriginalValue = rowVersion;
            Doctor? concurrencyDoctor = doctorToUpdate;
            if ( await TryUpdateModelAsync<Doctor>(doctorToUpdate,
                "",
                d => d.Name, d => d.Surname, d => d.Sepcialization, d => d.Email, d => d.Password))
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync<AppUser>(m => m.DoctorId == id);

                    await _context.SaveChangesAsync();
                    await _userManager.UpdateAsync(UserFromDoctorUpdate(doctorToUpdate, user));
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Doctor)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        concurrencyDoctor = (Doctor)databaseEntry.ToObject();
                        ModelState.AddModelError(string.Empty, "Data has been changed during your action. The newest version of data has been uploaded");
                        doctorToUpdate.RowVersion = (byte[])concurrencyDoctor.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }

            return View(concurrencyDoctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null || _context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Doctor doctor)
        {
         

            try
            {
                if (await _context.Doctor.AnyAsync(m => m.Id == doctor.Id))
                {
                    var doc = await _context.Doctor.FirstOrDefaultAsync(m => m.Id == doctor.Id);

                    var user = await _userManager.FindByEmailAsync(doc.Email);
                    _context.Doctor.Remove(doc);
                    await _userManager.DeleteAsync(user);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = doctor.Id});
            }
            //if (_context.Doctor == null)
            //{
            //    return Problem("Entity set 'ClinicAppContext.Doctor'  is null.");
            //}
            //var doctor = await _context.Doctor.FindAsync(id);
            //var user = await _userManager.FindByEmailAsync(doctor.Email);
            //if (doctor != null && user != null)
            //{
            //    _context.Doctor.Remove(doctor);
            //    await _userManager.DeleteAsync(user);
            //    await _context.SaveChangesAsync();
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Patient has been already deleted.");
            //}



            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Email(string Email, string password)
        {
            Doctor? doc = await _context.Doctor.Where(d => d.Email == Email)
                                               .FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                if (doc == null || doc.Password != password)
                {
                    ModelState.AddModelError("Password", "Invalid username or password");
                }
                else
                {
                    return RedirectToAction("Index", "Doctors");
                }
            }
            return View();
        }

        private bool DoctorExists(int id)
        {
            return (_context.Doctor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
