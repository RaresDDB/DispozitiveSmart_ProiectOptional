using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    
    public class ClientController : Controller
    {
        private readonly IDeviceService _deviceService;
        public ClientController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [Authorize(Roles = "client")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        public IActionResult Devices()
        {
            var userId = HttpContext.Request.Cookies["userId"];
            var userDevices = _deviceService.GetAllDevicesByUserId(userId);

            return View(new GetDevicesViewModel { Devices = userDevices.Result.Devices });
        }

        [Authorize(Roles = "client")]
        public IActionResult Measurements()
        {
            var deviceId = HttpContext.Request.Cookies["deviceId"];
            var intDeviceId = Int32.Parse(deviceId);

            var device = _deviceService.GetDeviceById(intDeviceId);

            return View(new GetMeasurementsViewModel { Device = device });
        }
    }
}
