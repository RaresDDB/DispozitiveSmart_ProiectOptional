using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.UserDevice;

namespace OnlineEnergyUtilityPlatform.Services.Interfaces
{
    public interface IDeviceService
    {
        List<GetDeviceDTO> GetAllDevices();
        GetDeviceDTO AddDevice(AddDeviceDTO device);
        GetDeviceDTO GetDeviceById(int id);
        UpdateDeviceDTO UpdateDevice(UpdateDeviceDTO device);
        void DeleteDevice(DeleteDeviceDTO device);
        Task<UserDevicesDTO> GetAllDevicesByUserId(string userId);
        Task AllocateUserToDevice(UserDeviceAllocateDTO userDeviceAllocate);
        void DeallocateUserToDevice(DeleteDeviceDTO device);
        List<GetDeviceDTO> GetAllDevicesWithoutUser();
        List<GetDeviceDTO> GetAllDevicesWithUserAlloc();
    }
}
