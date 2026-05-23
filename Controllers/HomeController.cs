using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YandexGoClone.Models;

namespace YandexGoClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Splash");

            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserName = user?.FullName ?? "Гость";
            ViewBag.Address = "Бишкек";
            return View();
        }

        [AllowAnonymous]
        public IActionResult Splash() => View();
    }
}