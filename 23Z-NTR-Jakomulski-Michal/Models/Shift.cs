using System.ComponentModel.DataAnnotations;
using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Shift
    {
        public int DoctorId { get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
    }
}
