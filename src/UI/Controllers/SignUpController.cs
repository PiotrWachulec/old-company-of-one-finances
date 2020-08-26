using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}