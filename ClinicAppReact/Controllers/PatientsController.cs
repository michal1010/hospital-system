using ClinicAppReact.Data;
using ClinicAppReact.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicAppReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
       
        private readonly ClinicAppContext _context;
        private readonly UserManager<AppUser> _userManager;


        public PatientsController(ClinicAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Doctorss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatient()
        {
            if (_context.Patient == null)
            {
                return NotFound();
            }
            IEnumerable<Patient> doctors = await _context.Patient.ToArrayAsync();
            return Ok(doctors);
        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient Patient)//string name, string sur, string spec, string mail, string pass)
        {
            if (_context.Patient == null)
            {
                return Problem("Entity set 'ClinicAppContext.Patient'  is null.");
            }

            _context.Patient.Add(Patient);
            await _context.SaveChangesAsync();
            var user = UserFromPatientCreate(Patient);
            var result = await _userManager.CreateAsync(user, Patient.Password);
            await _userManager.AddToRoleAsync(user, Constants.Roles.Pat.ToString());


            return CreatedAtAction("GetDoctor", new { id = Patient.Id }, Patient);
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

        //    // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            if (_context.Patient == null)
            {
                return NotFound();
            }
            var Patient = await _context.Patient.FindAsync(id);

            if (Patient == null)
            {
                return NotFound();
            }

            return Patient;
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient Patient)
        {
            if (id != Patient.Id)
            {
                return BadRequest();
            }


            _context.Entry(Patient).State = EntityState.Modified;
            var user = await _context.Users.FirstOrDefaultAsync<AppUser>(m => m.PatientId == id);


            try
            {
                await _userManager.UpdateAsync(UserFromPatientUpdate(Patient, user));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        public async Task<IActionResult> DeletePatient(int id)
        {
            if (_context.Patient == null)
            {
                return NotFound();
            }
            var Patient = await _context.Patient.FindAsync(id);
            if (Patient == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByEmailAsync(Patient.Email);

            await _userManager.DeleteAsync(user);

            _context.Patient.Remove(Patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return (_context.Patient?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
