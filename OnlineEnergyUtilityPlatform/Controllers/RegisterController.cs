using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            var baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            var loginUrl = baseUrl + "/Login";
            var registerViewModel = new RegisterViewModel {LoginUrl = loginUrl };

            return View(registerViewModel);
        }
    }
}
