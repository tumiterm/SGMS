using Microsoft.AspNetCore.Mvc;

namespace SGMS.Controllers
{
    public class Global : Controller
    {
        public IActionResult ResultsNotFound()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
