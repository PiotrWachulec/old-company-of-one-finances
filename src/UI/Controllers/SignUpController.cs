using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UI.Models.SignUp;

namespace UI.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNewUser(SignUpModel signUpModel)
        {
            return RedirectToAction("Index", "Home");
        }

    }
}