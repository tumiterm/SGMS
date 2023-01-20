using Microsoft.AspNetCore.Mvc;

namespace SGMS.Controllers
{
    public class GlobalController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
