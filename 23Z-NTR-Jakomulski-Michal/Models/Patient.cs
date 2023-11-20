using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class Patient : IdentityUser
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public string Login { get; set; }
        public string Password { get; set; }

        [Display(Name = "Account is active")]
        public bool IsAccountActive { get; set; }
    }
}
