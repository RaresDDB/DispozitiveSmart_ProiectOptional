using OnlineEnergyUtilityPlatform.Models;

namespace OnlineEnergyUtilityPlatform.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        List<Device> GetAllDevices();
        Device? GetDeviceById(int id);
        void AddDevice(Device device);
        void UpdateDevice(Device device);
        void RemoveDevice(Device device);  
        List<Device> GetAllDevicesByUserId(string userId);
        List<Device> GetAllDevicesWithoutUser();
        List<Device> GetAllDevicesWithUserAlloc();
    }
}
