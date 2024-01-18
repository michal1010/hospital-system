using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicAppReact.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClinicAppReact.Data
{
    public class ClinicAppContext : IdentityDbContext<AppUser>
    {
        public ClinicAppContext (DbContextOptions<ClinicAppContext> options)
            : base(options)
        {
        }

        public DbSet<ClinicAppReact.Models.Doctor> Doctor { get; set; } = default!;

        public DbSet<ClinicAppReact.Models.Patient> Patient { get; set; } = default!;

        public DbSet<ClinicAppReact.Models.Appointment> Appointment { get; set; } = default!;

    }
}
