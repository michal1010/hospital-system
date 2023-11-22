using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClinicApp.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Models;

public static class SeedData
{
    public enum Roles
    {
        Director,
        Patient,
        Doctor
    }
    public static async void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ClinicAppContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ClinicAppContext>>()))
        {
            // Look for any Doctors.
            if (!context.Doctor.Any())
            {
                context.Doctor.AddRange(
                new Doctor
                {
                    Name = "Jan",
                    Surname = "Kowalski",
                    Sepcialization = Doctor.Sepecializations.Oncology,
                    Email = "jan@gmail.com",
                    Password = "Password",
                   

                },
                new Doctor
                {
                    Name = "Olga",
                    Surname = "Nowak",
                    Sepcialization = Doctor.Sepecializations.Dermatology,
                    Email = "olg@gmail.com",
                    Password = "Password",

                },
                new Doctor
                {
                    Name = "Grzegorz",
                    Surname = "Braun",
                    Sepcialization = Doctor.Sepecializations.Internal,
                    Email = "grz@gmail.com",
                    Password = "Password",

                },
                new Doctor
                {
                    Name = "Juan",
                    Surname = "Świt",
                    Sepcialization = Doctor.Sepecializations.Dermatology,
                    Email = "hłan",
                    Password = "Password",
     
                }
            );
            }
            else if (!context.Patient.Any())
            {
                context.Patient.AddRange(
                new Patient
                {
                    Name = "Jan",
                    Surname = "Słoń",
                    Email = "jan@gmail.com",
                    Password = "Password",
                    IsAccountActive = true,

                },
                new Patient
                {
                    Name = "Olga",
                    Surname = "Rachoń",
                    Email = "olg@gmail.com",
                    Password = "Password",
                    IsAccountActive = true
                },
                new Patient
                {
                    Name = "Grzegorz",
                    Surname = "Turkacz",
                    Email = "grz@gmail.com",
                    Password = "Password",
                    IsAccountActive = false
                }
                );
            }
            else { return; }
           
            
            
            context.SaveChanges();
        }

        //var userManager = serviceProvider.GetService<UserManager<AppUser>>();
        //var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        //await roleManager.CreateAsync(new IdentityRole(Roles.Director.ToString()));
        //await roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));
        //await roleManager.CreateAsync(new IdentityRole(Roles.Patient.ToString()));
    }
}