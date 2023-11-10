using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class Doctor
    {
        // enum - with doctor specializations - TODO
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sepcialization { get; set; }
        public string Login{ get; set; }
        public string Password { get; set; }

        [Display(Name = "Join date")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

    }
}
