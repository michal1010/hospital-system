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
        public string Name { get; set; }
        public string Surname { get; set; }
        public Sepecializations Sepcialization { get; set; }
        public string Login{ get; set; }
        public string Password { get; set; }
        public bool IsDirector { get; set; }
    }
}
