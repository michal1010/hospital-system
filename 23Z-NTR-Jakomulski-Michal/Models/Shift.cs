namespace ClinicApp.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public Doctor? Doctor { get; set; }
        public int DoctorId {  get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
    }
}
