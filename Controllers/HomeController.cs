using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
