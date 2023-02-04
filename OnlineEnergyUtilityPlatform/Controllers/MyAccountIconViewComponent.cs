using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.ViewModels;

namespace OnlineEnergyUtilityPlatform.Controllers
{
    public class MyAccountIconViewComponent : ViewComponent
    {
        public async  Task<IViewComponentResult> InvokeAsync()
        {
            var computername = HttpContext.Request.Cookies["userName"];
            return View(new MyAccountIconViewModel { UserName = computername});
        }
    }
}
