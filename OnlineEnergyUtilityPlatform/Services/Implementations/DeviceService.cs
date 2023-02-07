using Microsoft.AspNetCore.Identity;
using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.User;
using OnlineEnergyUtilityPlatform.DTOs.UserDevice;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Models;
using OnlineEnergyUtilityPlatform.Repositories.Interfaces;
using OnlineEnergyUtilityPlatform.Services.Interfaces;

namespace OnlineEnergyUtilityPlatform.Services.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMeasurementRepository _measurementRepository;

        public DeviceService(IDeviceRepository deviceRepository, UserManager<User> userManager, IMeasurementRepository measurementRepository)
        {
            _deviceRepository = deviceRepository;
            _userManager = userManager;
            _measurementRepository = measurementRepository;
        }

        public List<GetDeviceDTO> GetAllDevices()
        {
            var devicesDTO = new List<GetDeviceDTO>();

            foreach (var device in _deviceRepository.GetAllDevices())
            {
                string username;
                if(device.UserId != null)
                {
                    var user = _userManager.FindByIdAsync(device.UserId);
                    username = user.Result.UserName;
                }
                else
                {
                    username = "nil";
                }

                devicesDTO.Add(new GetDeviceDTO
                {
                    id = device.Id, 
                    name = device.Name,
                    description = device.Description,
                    address = device.Address,
                    maximumconsumption = device.MaximumConsumption,
                    userid = device.UserId,
                    username = username,    
                });
            }

            return devicesDTO;
        }

        public GetDeviceDTO AddDevice(AddDeviceDTO device)
        {
            if(string.IsNullOrEmpty(device.Name))
            {
                throw new DeviceException("The name of device is requierd");
            }

            if(string.IsNullOrEmpty(device.Description))
            {
                throw new DeviceException("The description of device is requierd");
            }

            if (string.IsNullOrEmpty(device.Address))
            {
                throw new DeviceException("The address of device is requierd");
            }

            if (device.MaximumConsumption <= 0)
            {
                throw new DeviceException("The maximum consumption of device must be a positive number!");
            }

            _deviceRepository.AddDevice(new Device
            {
                Name = device.Name,
                Description = device.Description,
                Address = device.Address,
                MaximumConsumption = device.MaximumConsumption
            });

            var deviceDTO = new GetDeviceDTO
            {
                name = device.Name,
                description= device.Description,
                address = device.Address,
                maximumconsumption = device.MaximumConsumption,
                username = "nil"
            };

            return deviceDTO;
        }

        public GetDeviceDTO GetDeviceById(int id)
        {
            var currentDevice = _deviceRepository.GetDeviceById(id);

            if(currentDevice == null)
            {
                throw new DeviceException("The device doesn't exit!");
            }

            var deviceById = new GetDeviceDTO
            {
                id = currentDevice.Id,
                name = currentDevice.Name,
                description = currentDevice.Description,
                address = currentDevice.Address,    
                maximumconsumption= currentDevice.MaximumConsumption,   
                userid = currentDevice.UserId
            };

            return deviceById;
        }

        public UpdateDeviceDTO UpdateDevice(UpdateDeviceDTO device)
        {
            var currentDevice = _deviceRepository.GetDeviceById(device.Id);

            if(currentDevice == null)
            {
                throw new DeviceException("The device doesn't exist!");
            }

            if (string.IsNullOrEmpty(device.Name))
            {
                throw new DeviceException("The name of device is requierd");
            }

            if (string.IsNullOrEmpty(device.Description))
            {
                throw new DeviceException("The description of device is requierd");
            }

            if (string.IsNullOrEmpty(device.Address))
            {
                throw new DeviceException("The address of device is requierd");
            }

            if (device.MaximumConsumption <= 0)
            {
                throw new DeviceException("The maximum consumption of device must be a positive number!");
            }

            if(currentDevice.Name.Equals(device.Name) && currentDevice.Description.Equals(device.Description) 
                && currentDevice.Address.Equals(device.Address) && currentDevice.MaximumConsumption == device.MaximumConsumption)
            {
                throw new DeviceException("The device values has not changed!");
            }

            currentDevice.Name = device.Name;
            currentDevice.Description = device.Description;
            currentDevice.Address = device.Address;
            currentDevice.MaximumConsumption = device.MaximumConsumption;

            _deviceRepository.UpdateDevice(currentDevice);

            return device;
        }

        public void DeleteDevice(DeleteDeviceDTO device)
        {
            var deleteDevice = _deviceRepository.GetDeviceById(device.Id);

            if(deleteDevice == null)
            {
                throw new DeviceException("The device doesn't exist!");
            }

            var measurements = _measurementRepository.GetAllMeasurementsByDevice(device.Id);

            if(measurements != null)
            {
                foreach(var measurement in measurements)
                {
                    _measurementRepository.DeleteMeasurement(measurement);
                }
            }

            _deviceRepository.RemoveDevice(deleteDevice);

        }

        public async Task<UserDevicesDTO> GetAllDevicesByUserId(string userId)
        {
            var devicesDTO = new List<GetDeviceDTO>();
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                throw new DeviceException("The current user are not available!");
            }

            foreach (var device in _deviceRepository.GetAllDevicesByUserId(userId))
            {
                devicesDTO.Add(new GetDeviceDTO
                {
                    id = device.Id,
                    name = device.Name,
                    description = device.Description,
                    address = device.Address,
                    maximumconsumption = device.MaximumConsumption,
                    userid = device.UserId
                });
            }

            var userDTO = new GetUserDTO
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email
            };

            return new UserDevicesDTO { User = userDTO, Devices = devicesDTO};
        }

        public async Task AllocateUserToDevice(UserDeviceAllocateDTO userDeviceAllocate)
        {
            var user = await _userManager.FindByIdAsync(userDeviceAllocate.UserId);

            if(user == null)
            {
                throw new DeviceException("The current user are not available for allocation!");
            }

            var device = _deviceRepository.GetDeviceById(userDeviceAllocate.DeviceId);

            if(device == null)
            {
                throw new DeviceException("The current device doesn't exist!");
            }

            if(device.User != null)
            {
                throw new DeviceException("The current device are not available for allocation!");
            }

            device.UserId = user.Id;
            device.User = user;

            _deviceRepository.UpdateDevice(device);
        }

        public void DeallocateUserToDevice(DeleteDeviceDTO deviceDTO)
        {
            var device = _deviceRepository.GetDeviceById(deviceDTO.Id);

            if (device == null)
            {
                throw new DeviceException("The current device doesn't exist!");
            }

            device.UserId = null;
            device.User = null;

            _deviceRepository.UpdateDevice(device);
        }

        public List<GetDeviceDTO> GetAllDevicesWithoutUser()
        {
            var devicesDTO = new List<GetDeviceDTO>();

            foreach (var device in _deviceRepository.GetAllDevicesWithoutUser())
            {
                devicesDTO.Add(new GetDeviceDTO
                {
                    id = device.Id,
                    name = device.Name,
                    description = device.Description,
                    address = device.Address,
                    maximumconsumption = device.MaximumConsumption,
                    userid = device.UserId
                });
            }

            return devicesDTO;
        }

        public List<GetDeviceDTO> GetAllDevicesWithUserAlloc()
        {
            var devicesDTO = new List<GetDeviceDTO>();

            foreach(var device in _deviceRepository.GetAllDevicesWithUserAlloc())
            {
                devicesDTO.Add(new GetDeviceDTO
                {
                    id = device.Id,
                    name = device.Name,
                    description = device.Description,
                    address = device.Address,
                    maximumconsumption = device.MaximumConsumption,
                });
            }

            return devicesDTO;
        }
    }
}
