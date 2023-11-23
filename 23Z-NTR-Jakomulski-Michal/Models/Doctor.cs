using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class Doctor
    {
        // enum - with doctor specializations - TODO
        public enum Sepecializations
        {
            Internal,
            Pediartrics,
            Neurology,
            Oncology,
            Dermatology,
            Cariology,
            Surgeon
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public Sepecializations Sepcialization { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email{ get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool IsDirector { get; set; }

        public AppUser? AppUser { get; set; }
    }
}
