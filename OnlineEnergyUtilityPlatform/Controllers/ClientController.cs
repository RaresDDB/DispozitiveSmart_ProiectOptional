using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    
    public class ClientController : Controller
    {

        public ClientController()
        {
 
        }

        [Authorize(Roles = "client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
