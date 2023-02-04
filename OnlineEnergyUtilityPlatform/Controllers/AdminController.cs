using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {
            var users = _userService.GetAllUsers();
            return View(new GetUsersViewModel { Users = users });
        }
    }
}
