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
            if (!context.Doctor.Any())
            {
                context.Doctor.AddRange(
                new Doctor
                {
                    Name = "Jan",
                    Surname = "Kowalski",
                    Sepcialization = Doctor.Sepecializations.Oncology,
                    Login = "jan",
                    Password = "Password",
                   

                },
                new Doctor
                {
                    Name = "Olga",
                    Surname = "Nowak",
                    Sepcialization = Doctor.Sepecializations.Dermatology,
                    Login = "olg",
                    Password = "Password",

                },
                new Doctor
                {
                    Name = "Grzegorz",
                    Surname = "Braun",
                    Sepcialization = Doctor.Sepecializations.Internal,
                    Login = "grz",
                    Password = "Password",

                },
                new Doctor
                {
                    Name = "Juan",
                    Surname = "Świt",
                    Sepcialization = Doctor.Sepecializations.Dermatology,
                    Login = "hłan",
                    Password = "Password",
     
                }
            );
            }
            else if( !context.Patient.Any())
            {
                context.Patient.AddRange(
                new Patient
                {
                    Name = "Jan",
                    Surname = "Słoń",
                    Login = "jan",
                    Password = "Password",
                    IsAccountActive = true,

                },
                new Patient
                {
                    Name = "Olga",
                    Surname = "Rachoń",
                    Login = "olg",
                    Password = "Password",
                    IsAccountActive = true
                },
                new Patient
                {
                    Name = "Grzegorz",
                    Surname = "Turkacz",
                    Login = "grz",
                    Password = "Password",
                    IsAccountActive = false
                }
                );
            }
            else { return; }
           
            
            
            context.SaveChanges();
        }
    }
}