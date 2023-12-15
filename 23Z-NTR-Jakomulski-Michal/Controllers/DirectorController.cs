using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using ClinicApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

namespace ClinicApp.Controllers
{
    [Authorize(Roles = "Director")]
    public class DirectorController : Controller
    {
        private readonly IConfiguration _configuration;

        public DirectorController(IConfiguration context)
        {
            _configuration = context;
        }

        // GET: Doctors
        public IActionResult Index()
        {
            string? projectDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            System.IO.File.Delete(projectDir + "\\raport.pdf");
            return View();
        }
    }
}
