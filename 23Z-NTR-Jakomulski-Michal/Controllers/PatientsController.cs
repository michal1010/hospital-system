using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Data;
using ClinicApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using ClinicApp.Migrations;

namespace ClinicApp.Controllers
{

    public class PatientsController : Controller
    {
        private readonly ClinicAppContext _context;
        private readonly UserManager<AppUser> _userManager;


        public PatientsController(ClinicAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
              return _context.Patient != null ? 
                          View(await _context.Patient.ToListAsync()) :
                          Problem("Entity set 'ClinicAppContext.Pat'  is null.");
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Password,IsAccountActive")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                var user = UserFromPatientCreate(patient);
                var result = await _userManager.CreateAsync(user, patient.Password);
                await _userManager.AddToRoleAsync(user, Constants.Roles.Pat.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, byte[] rowVersion)
        {

            var patientToUpdate = await _context.Patient.FirstOrDefaultAsync(d => d.Id == id);

            if (patientToUpdate == null)
            {
                Patient deletedPatient = new Patient();
                await TryUpdateModelAsync(deletedPatient);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                return View(deletedPatient);
            }

            _context.Entry(patientToUpdate).Property("RowVersion").OriginalValue = rowVersion;
            Patient? concurrencyPatient = patientToUpdate;
            if (await TryUpdateModelAsync<Patient>(patientToUpdate,
                "",
                d => d.Name, d => d.Surname, d => d.Email, d => d.Password))
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync<AppUser>(m => m.PatientId == id);

                    await _context.SaveChangesAsync();
                    await _userManager.UpdateAsync(UserFromPatientUpdate(patientToUpdate, user));
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Patient)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        concurrencyPatient = (Patient)databaseEntry.ToObject();
                        ModelState.AddModelError(string.Empty, "Data has been changed during your action. The newest version of data has been uploaded");
                        patientToUpdate.RowVersion = (byte[])concurrencyPatient.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }

            return View(concurrencyPatient);
           
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patient == null)
            {
                return Problem("Entity set 'ClinicAppContext.Pat'  is null.");
            }
            var patient = await _context.Patient.FindAsync(id);
            var user = await _userManager.FindByEmailAsync(patient.Email);

            if (patient != null && user != null)
            {
                _context.Patient.Remove(patient);
                await _userManager.DeleteAsync(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return (_context.Patient?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private AppUser UserFromPatientCreate(Patient patient)
        {
            AppUser au = Activator.CreateInstance<AppUser>();

            au.UserName = patient.Email;
            au.Name = patient.Name;
            au.Surname = patient.Surname;
            au.Email = patient.Email;
            au.EmailConfirmed = true;
            au.PhoneNumberConfirmed = true;
            au.Patient = patient;
            return au;
        }

        private AppUser UserFromPatientUpdate(Patient patient, AppUser user)
        {

            user.UserName = patient.Email;
            user.Name = patient.Name;
            user.Surname = patient.Surname;
            user.Email = patient.Email;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.Patient = patient;
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccount(int? id)
        {

            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            patient.IsAccountActive = true;
            AppUser? user = await _userManager.FindByEmailAsync(patient.Email);

            await _userManager.AddToRoleAsync(user, Constants.Roles.Pat.ToString());
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
           
        }
    }
}
