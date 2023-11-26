namespace ClinicApp.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public Doctor? doctor { get; set; }
        public int? DoctorId { get; set; }
        public Patient? Patient { get; set; }
        public int? PatientId { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
