using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    public class EntriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
