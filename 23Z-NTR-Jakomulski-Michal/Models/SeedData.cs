using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClinicApp.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClinicApp.Models;

public static class SeedData
{
    public enum Roles
    {
        Director,
        Patient,
        Doctor
    }
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        //using (var context = new ClinicAppContext(
        //    serviceProvider.GetRequiredService<
        //        DbContextOptions<ClinicAppContext>>()))
        //{
        //    // Look for any Doctors.
        //    if (!context.Doctor.Any())
        //    {
        //        context.Doctor.AddRange(
        //        new Doctor
        //        {
        //            Name = "Jan",
        //            Surname = "Kowalski",
        //            Sepcialization = Doctor.Sepecializations.Oncology,
        //            Email = "jan@gmail.com",
        //            Password = "Password",
                   

        //        },
        //        new Doctor
        //        {
        //            Name = "Olga",
        //            Surname = "Nowak",
        //            Sepcialization = Doctor.Sepecializations.Dermatology,
        //            Email = "olg@gmail.com",
        //            Password = "Password",

        //        },
        //        new Doctor
        //        {
        //            Name = "Grzegorz",
        //            Surname = "Braun",
        //            Sepcialization = Doctor.Sepecializations.Internal,
        //            Email = "grz@gmail.com",
        //            Password = "Password",

        //        },
        //        new Doctor
        //        {
        //            Name = "Juan",
        //            Surname = "Świt",
        //            Sepcialization = Doctor.Sepecializations.Dermatology,
        //            Email = "hłan",
        //            Password = "Password",
     
        //        }
        //    );
        //    }
        //    else if (!context.Patient.Any())
        //    {
        //        context.Patient.AddRange(
        //        new Patient
        //        {
        //            Name = "Jan",
        //            Surname = "Słoń",
        //            Email = "jan@gmail.com",
        //            Password = "Password",
        //            IsAccountActive = true,

        //        },
        //        new Patient
        //        {
        //            Name = "Olga",
        //            Surname = "Rachoń",
        //            Email = "olg@gmail.com",
        //            Password = "Password",
        //            IsAccountActive = true
        //        },
        //        new Patient
        //        {
        //            Name = "Grzegorz",
        //            Surname = "Turkacz",
        //            Email = "grz@gmail.com",
        //            Password = "Password",
        //            IsAccountActive = false
        //        }
        //        );
        //    }
        //    else { ; }
           
            
            
        //    //context.SaveChanges();
        //}

        var userManager = serviceProvider.GetService<UserManager<AppUser>>();
        //var userStore = serviceProvider.GetService<IUserStore<AppUser>>();
        //var emailStore = (IUserEmailStore<AppUser>)userStore;
        ////var signInManager = serviceProvider.GetService<SignInManager<AppUser>>();

        //var user = Activator.CreateInstance<AppUser>();
        //user.Name = "Jan";
        //user.Surname = "Słoń";
        //await userStore.SetUserNameAsync(user, "jan@gmail.com", CancellationToken.None);
        //await emailStore.SetEmailAsync(user, "jan@gmail.com", CancellationToken.None);
        //var result = await userManager.CreateAsync(user, "P@ssword1");

        Doctor doc = new Doctor
        {
            Name = "Jan",
            Surname = "Kowalski",
            Sepcialization = Doctor.Sepecializations.Oncology,
            Email = "jan@gmail.com",
            Password = "P@ssword1",
        };

        using (var context = new ClinicAppContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ClinicAppContext>>()))
        {
            context.Add<Doctor>(doc);
        }
            List<AppUser> users = new List<AppUser>()
        {
            new AppUser
            {
                    UserName = "jan@gmail.com",
                    Name = "Jan",
                    Surname = "Kowalski",
                    Email = "jan@gmail.com",
                    //PasswordHash = "P@ssword1",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Doctor = doc
            },
            //new AppUser
            //{
            //        UserName = "olg@gmail.com",
            //        Name = "Olga",
            //        Surname = "Nowak",
            //        Email = "olg@gmail.com",
            //        //PasswordHash = "P@ssword1",
            //         EmailConfirmed = true,
            //        PhoneNumberConfirmed = true
            //},
            //new AppUser
            //{
            //    UserName = "grz@gmail.com",
            //        Name = "Grzegorz",
            //        Surname = "Braun",
            //        Email = "grz@gmail.com",
            //        //PasswordHash = "P@ssword1",
            //         EmailConfirmed = true,
            //        PhoneNumberConfirmed = true
            //}
        };

        foreach (var user in users)
        {

            await userManager.CreateAsync(user, "P@ssword1");
        }
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Director.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Patient.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.Director.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }
        await userManager.AddToRoleAsync(users[0], Roles.Director.ToString());
    }
}