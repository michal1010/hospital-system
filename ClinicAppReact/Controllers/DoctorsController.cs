using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicAppReact.Data;
using ClinicAppReact.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicAppReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ClinicAppContext _context;
        private readonly UserManager<AppUser> _userManager;


        public DoctorsController(ClinicAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctor()
        {
            if (_context.Doctor == null)
            {
                return NotFound();
            }
            IEnumerable<Doctor> doctors =  await _context.Doctor.ToArrayAsync();
            return Ok(doctors);
        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)//string name, string sur, string spec, string mail, string pass)
        {
            if (_context.Doctor == null)
            {
                return Problem("Entity set 'ClinicAppContext.Doctor'  is null.");
            }

            _context.Doctor.Add(doctor);
            await _context.SaveChangesAsync();
            var user = UserFromDoctorCreate(doctor);
            var result = await _userManager.CreateAsync(user, doctor.Password);
            await _userManager.AddToRoleAsync(user, Constants.Roles.Doctor.ToString());
           

            return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
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

        //    // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            if (_context.Doctor == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctor.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }


            _context.Entry(doctor).State = EntityState.Modified;
            var user = await _context.Users.FirstOrDefaultAsync<AppUser>(m => m.DoctorId == id);


            try
            {
                await _userManager.UpdateAsync(UserFromDoctorUpdate(doctor, user));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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



        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            if (_context.Doctor == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByEmailAsync(doctor.Email);

            await _userManager.DeleteAsync(user);

            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return (_context.Doctor?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
