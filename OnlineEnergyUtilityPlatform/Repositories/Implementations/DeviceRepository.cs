using OnlineEnergyUtilityPlatform.Data;
using OnlineEnergyUtilityPlatform.Models;
using OnlineEnergyUtilityPlatform.Repositories.Interfaces;

namespace OnlineEnergyUtilityPlatform.Repositories.Implementations
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly EnergyPlatformDbContext _energyPlatformDbContext;

        public DeviceRepository(EnergyPlatformDbContext energyPlatformDbContext)
        {
            _energyPlatformDbContext = energyPlatformDbContext;
        }

        public void AddDevice(Device device)
        {
            _energyPlatformDbContext.Device.Add(device);
            _energyPlatformDbContext.SaveChanges();
        }

        public List<Device> GetAllDevices()
        {
            return _energyPlatformDbContext.Device.ToList();
        }

        public List<Device> GetAllDevicesByUserId(string userId)
        {
            return _energyPlatformDbContext.Device.Where(x => x.UserId.Equals(userId)).ToList();
        }

        public List<Device> GetAllDevicesWithoutUser()
        {
           return _energyPlatformDbContext.Device.Where(x => string.IsNullOrEmpty(x.UserId)).ToList();
        }

        public List<Device> GetAllDevicesWithUserAlloc()
        {
            return _energyPlatformDbContext.Device.Where(x => !string.IsNullOrEmpty(x.UserId)).ToList();
        }

        public Device? GetDeviceById(int id)
        {
            return _energyPlatformDbContext.Device.SingleOrDefault(x => x.Id == id);
        }

        public void RemoveDevice(Device device)
        {
            _energyPlatformDbContext.Device.Remove(device);
            _energyPlatformDbContext.SaveChanges();
        }

        public void UpdateDevice(Device device)
        {
            _energyPlatformDbContext.Device.Update(device);
            _energyPlatformDbContext.SaveChanges();
        }
    }
}
