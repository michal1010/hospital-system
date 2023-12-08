using System.ComponentModel.DataAnnotations;
using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public Doctor? Doctor { get; set; }
        public int DoctorId {  get; set; }
        public Patient? Patient { get; set; }
        public int? PatientId { get; set; }
        
        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = $"{{0:{Constants.DATE_FORMAT}}}", ApplyFormatInEditMode = true)]
        public DateTime StartTime {  get; set; }
    }
}
