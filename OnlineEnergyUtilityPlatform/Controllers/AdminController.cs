using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;

        public AdminController(IUserService userService, IDeviceService deviceService)
        {
            _userService = userService;
            _deviceService = deviceService;
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

        [Authorize(Roles = "admin")]
        public IActionResult Devices()
        {
            var devices = _deviceService.GetAllDevices();
            return View(new GetDevicesViewModel { Devices = devices });
        }

        [Authorize(Roles = "admin")]
        public IActionResult AllocateDevice()
        {
            var users = _userService.GetAllUsers();
            var devices = _deviceService.GetAllDevicesWithoutUser();

            return View(new AllocateUsersDevicesViewModel { Users = users, Devices = devices });
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeallocateDevice()
        {
            var devices = _deviceService.GetAllDevicesWithUserAlloc();

            return View(new DeallocateUserDeviceViewModel { Devices = devices });
        }
    }
}
