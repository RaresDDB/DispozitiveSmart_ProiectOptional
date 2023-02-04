using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineEnergyUtilityPlatform.Models
{
    public class UserToken : IdentityUserToken<string>
    {
    }
}
