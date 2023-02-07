using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.User;

namespace OnlineEnergyUtilityPlatform.ViewModels
{
    public class AllocateUsersDevicesViewModel
    {
        public List<GetUserDTO> Users { get; set; }
        public List<GetDeviceDTO> Devices { get; set; }
    }
}
