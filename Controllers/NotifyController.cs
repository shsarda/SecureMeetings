using Microsoft.AspNetCore.Mvc;

namespace SecureMeetings.Controllers
{
    public class NotifyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
