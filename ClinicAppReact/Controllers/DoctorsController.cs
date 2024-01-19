using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicAppReact.Data;
using ClinicAppReact.Models;

namespace ClinicAppReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ClinicAppContext _context;


        public DoctorsController(ClinicAppContext context)
        {
            _context = context;
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

        //    // GET: api/Doctors/5
        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<Doctor>> GetDoctor(int id)
        //    {
        //      if (_context.Doctor == null)
        //      {
        //          return NotFound();
        //      }
        //        var doctor = await _context.Doctor.FindAsync(id);

        //        if (doctor == null)
        //        {
        //            return NotFound();
        //        }

        //        return doctor;
        //    }

        //    // PUT: api/Doctors/5
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        //    {
        //        if (id != doctor.Id)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(doctor).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DoctorExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/Doctors
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPost]
        //    public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        //    {
        //      if (_context.Doctor == null)
        //      {
        //          return Problem("Entity set 'ClinicAppContext.Doctor'  is null.");
        //      }
        //        _context.Doctor.Add(doctor);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        //    }

        //    // DELETE: api/Doctors/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteDoctor(int id)
        //    {
        //        if (_context.Doctor == null)
        //        {
        //            return NotFound();
        //        }
        //        var doctor = await _context.Doctor.FindAsync(id);
        //        if (doctor == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Doctor.Remove(doctor);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //    private bool DoctorExists(int id)
        //    {
        //        return (_context.Doctor?.Any(e => e.Id == id)).GetValueOrDefault();
        //    }
        //}
    }
}
