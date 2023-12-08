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
        public async Task<IActionResult> Create([Bind("Id,Name,Surname, Sepcialization,Email,Password")] Doctor doctor)
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

            var doctor = await _context.Doctor.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Sepcialization,Email,Password")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync<AppUser>(m => m.DoctorId == id );

                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                    
                    await _userManager.UpdateAsync(UserFromDoctorUpdate(doctor, user));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctor == null)
            {
                return Problem("Entity set 'ClinicAppContext.Doctor'  is null.");
            }
            var doctor = await _context.Doctor.FindAsync(id);
            var user = await _userManager.FindByEmailAsync(doctor.Email);
            if (doctor != null && user != null)
            {
                _context.Doctor.Remove(doctor);
                await _userManager.DeleteAsync(user);
            }

            await _context.SaveChangesAsync();
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
