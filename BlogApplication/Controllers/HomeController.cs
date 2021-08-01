using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private IEmailService _emailService;

        public HomeController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EmailPost(EmailModel semail)
        {
            if (ModelState.IsValid)
            {
                _emailService.AddAsync(semail.EmailAddress);
                return View();
            }
            return View("Error");
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
