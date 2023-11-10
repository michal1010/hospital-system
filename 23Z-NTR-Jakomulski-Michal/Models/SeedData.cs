using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClinicApp.Data;
using System;
using System.Linq;

namespace ClinicApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ClinicAppContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ClinicAppContext>>()))
        {
            // Look for any Doctors.
            if (context.Doctor.Any())
            {
                return;   // DB has been seeded
            }
            context.Doctor.AddRange(
                new Doctor
                {
                    Name = "Jan",
                    Sepcialization = "dermatolog",
                    Login = "jan",
                    Password = "Password",
                    JoinDate = DateTime.Now,

                },
                new Doctor
                {
                    Name = "Olga",
                    Sepcialization = "dermatolog",
                    Login = "olg",
                    Password = "Password",
                    JoinDate = DateTime.Now,
                },
                new Doctor
                {
                    Name = "Grzegorz",
                    Sepcialization = "dermatolog",
                    Login = "grz",
                    Password = "Password",
                    JoinDate = DateTime.Now,
                },
                new Doctor
                {
                    Name = "Juan",
                    Sepcialization = "dermatolog",
                    Login = "hłan",
                    Password = "Password",
                    JoinDate = DateTime.Now,
                }
            );
            
            context.SaveChanges();
        }
    }
}