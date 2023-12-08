using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClinicApp.Models;

namespace ClinicApp.Data;

public static class SeedData
{

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        Parameters.GetInstance();

        var userManager = serviceProvider.GetService<UserManager<AppUser>>();

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
            context.Add(doc);
        }
        List<AppUser> users = new List<AppUser>()
        {
            new AppUser
            {
                    UserName = "jan@gmail.com",
                    Name = "Jan",
                    Surname = "Kowalski",
                    Email = "jan@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Doctor = doc
            },
        };


        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        if (roleManager == null) throw new Exception("Role manager is null");

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Director.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Doctor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Patient.ToString())); ;
        }


        if (userManager == null)
            throw new Exception("User Manger is null");

        if (!userManager.Users.Any())
        {
            foreach (var user in users)
            {

                await userManager.CreateAsync(user, "P@ssword1");
            }
            await userManager.AddToRoleAsync(users[0], Constants.Roles.Director.ToString());

        }


    }
}