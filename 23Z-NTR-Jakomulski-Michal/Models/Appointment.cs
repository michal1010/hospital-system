using System.ComponentModel.DataAnnotations;
using System.Text;
using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public Doctor? Doctor { get; set; }
        public int DoctorId { get; set; }
        public Patient? Patient { get; set; }
        public int? PatientId { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Start time")]
        [DisplayFormat(DataFormatString = $"{{0:{Constants.DATE_FORMAT}}}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Doctor:").Append(Doctor.Name).Append(" ").
                Append(Doctor.Surname).Append(" Date").Append(StartTime);

            if (PatientId != null)
            {
                sb.Append(" Patient").Append(Patient.Name).Append(" ").Append(Patient.Surname);
            }

            if (Description != null)
            {
                sb.Append(Description).Append("\n--------------------------\n");
            }

            return sb.ToString();
            
        }
    }
}
