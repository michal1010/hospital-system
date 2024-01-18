using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ClinicAppReact.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Doctor? Doctor { get; set; }
        public int? DoctorId { get; set; }
        public Patient? Patient { get; set; }   
        public int? PatientId { get; set; }
    }
}
