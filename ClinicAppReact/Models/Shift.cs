using System.ComponentModel.DataAnnotations;
using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Shift
    {
        public int DoctorId { get; set; }

        [DataType(DataType.DateTime)]
        //[DivisibleBy15(ErrorMessage = "Number of minutes must be divisible by 15.")]
        public DateTime StartTime {  get; set; }

        [DataType(DataType.DateTime)]
        //[DivisibleBy15(ErrorMessage = "Number of minutes must be divisible by 15.")]
        //[StartTimeBeforeEndTime("StartTime", ErrorMessage = "End time must not be earlier than start time.")]
        public DateTime EndTime { get; set; }
    }
}
