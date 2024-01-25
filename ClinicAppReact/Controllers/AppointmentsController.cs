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
    public class AppointmentsController : ControllerBase
    {
        private readonly ClinicAppContext _context;

        public AppointmentsController(ClinicAppContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment()
        {
              if (_context.Appointment == null)
              {
                  return NotFound();
              }
            IEnumerable<AppointmentDTO> appointments = await _context.Appointment
                                                                  .Include(app => app.Doctor)
                                                                  .Select(app => new AppointmentDTO
                                                                  {
                                                                      Id = app.Id,
                                                                      Doctor = new DoctorDTO
                                                                      {
                                                                          Id = app.Doctor.Id,
                                                                          Name = app.Doctor.Name,
                                                                          Surname = app.Doctor.Surname,
                                                                          Sepcialization = app.Doctor.Sepcialization
                                                                      },
                                                                      StartTime = app.StartTime
                                                                  }).ToArrayAsync();
            return Ok(appointments);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
          if (_context.Appointment == null)
          {
              return NotFound();
          }
            var appointment = await _context.Appointment.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        public async Task CreateAppointment(Appointment appointment)
        {

            await _context.Appointment.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        private DateTime convertDate(string  dateString)
        {
            //string dateString = "2024-01-25T15:25";

            // Specify the format of the input string
            string format = "yyyy-MM-ddTHH:mm";

            // Parse the string into a DateTime object
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, format,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dateTime))
            {
                // Now you can work with the parsed DateTime object
                return dateTime;
            }
            else
            {
                return DateTime.MinValue;
            }

        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            //DateTime startTimeD = convertDate(startTime);
            //DateTime endTimeD = convertDate(endTime);

            //Shift shift = new Shift {
            //    Id = doctorId, // TODO Handle better shift id
            //    DoctorId = doctorId,
            //    StartTime = startTimeD,
            //    EndTime = endTimeD
            //};
            

            if (_context.Appointment == null)
            {
                return Problem("Entity set 'Shift'  is null.");
            }
            
            await CreateAppointments(shift);
            Doctor? doc = _context.Doctor.Include(d => d.Appointments).FirstOrDefault((d) => d.Id == shift.Id);
            Appointment? app = doc.Appointments.First();
            return CreatedAtAction("GetAppointment", new { id = app.Id }, app);
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
                    await CreateAppointment(new Appointment
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

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (_context.Appointment == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return (_context.Appointment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
