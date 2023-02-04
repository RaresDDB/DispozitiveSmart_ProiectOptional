using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineEnergyUtilityPlatform.Models
{
    public class UserRole : IdentityUserRole<string>
    {
    }
}
