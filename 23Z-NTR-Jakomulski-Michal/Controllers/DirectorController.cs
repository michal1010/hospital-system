using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using ClinicApp.Data;

namespace ClinicApp.Controllers
{
    public class DirectorController : Controller
    {
        //private readonly ClinicAppContext _context;

        //public DirectorController(ClinicAppContext context)
        //{
        //    _context = context;
        //}

        // GET: Doctors
        public IActionResult Index()
        {
            return View();
        }
    }
}
