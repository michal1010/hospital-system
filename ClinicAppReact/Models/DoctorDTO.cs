using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using static ClinicAppReact.Models.Doctor;

namespace ClinicAppReact.Models
{
    public class DoctorDTO
    { 
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        [Required]
        public Sepecializations Sepcialization { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        public bool IsDirector { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }

        //[Timestamp]
        //public byte[]? RowVersion { get; set; }

        public override string? ToString()
        {
            return Name + " " + Surname;
        }
    }
}
