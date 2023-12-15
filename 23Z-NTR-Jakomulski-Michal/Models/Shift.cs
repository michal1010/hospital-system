using System.ComponentModel.DataAnnotations;
using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Shift
    {
        public int DoctorId { get; set; }

        [DivisibleBy15(ErrorMessage = "Number of minutes must be divisible by 15.")]
        public DateTime StartTime {  get; set; }

        [DivisibleBy15(ErrorMessage = "Number of minutes must be divisible by 15.")]
        public DateTime EndTime { get; set; }
    }
}
