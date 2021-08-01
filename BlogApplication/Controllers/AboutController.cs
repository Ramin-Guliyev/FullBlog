using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
