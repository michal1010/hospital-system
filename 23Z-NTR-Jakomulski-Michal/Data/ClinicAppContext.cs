using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClinicApp.Data
{
    public class ClinicAppContext : IdentityDbContext<AppUser>
    {
        public ClinicAppContext (DbContextOptions<ClinicAppContext> options)
            : base(options)
        {
        }

        public DbSet<ClinicApp.Models.Doctor> Doctor { get; set; } = default!;

        public DbSet<ClinicApp.Models.Patient> Patient { get; set; } = default!;

        public DbSet<ClinicApp.Models.Visit> Visit { get; set; } = default!;

        public DbSet<ClinicApp.Models.Shift> Shift { get; set; } = default!;
    }
}
