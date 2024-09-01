using Microsoft.AspNetCore.Mvc;

namespace ABC_Company.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return RedirectToAction("Index", "LogIn");
        }
    }
}
