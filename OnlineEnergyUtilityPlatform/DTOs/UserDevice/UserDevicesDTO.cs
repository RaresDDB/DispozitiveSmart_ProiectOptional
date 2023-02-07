using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.User;

namespace OnlineEnergyUtilityPlatform.DTOs.UserDevice
{
    public class UserDevicesDTO
    {
        public GetUserDTO User { get; set; }
        public List<GetDeviceDTO> Devices { get; set; }
    }
}
