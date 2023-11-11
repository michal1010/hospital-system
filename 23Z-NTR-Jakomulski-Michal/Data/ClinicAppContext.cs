using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;

namespace ClinicApp.Data
{
    public class ClinicAppContext : DbContext
    {
        public ClinicAppContext (DbContextOptions<ClinicAppContext> options)
            : base(options)
        {
        }

        public DbSet<ClinicApp.Models.Doctor> Doctor { get; set; } = default!;

        public DbSet<ClinicApp.Models.Patient> Patient { get; set; } = default!;
    }
}
