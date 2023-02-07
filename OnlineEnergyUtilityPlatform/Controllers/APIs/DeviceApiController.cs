using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.UserDevice;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Services.Interfaces;

namespace OnlineEnergyUtilityPlatform.Controllers.APIs
{
    [Route("/api/[controller]")]
    [ApiController]
    public class DeviceApiController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceApiController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("GetAllDevices")]
        public IActionResult GetAllDevices()
        {
            return Ok(_deviceService.GetAllDevices());
        }

        [HttpPost("AddDevice")]
        [Authorize(Roles = "admin")]
        public IActionResult AddDevice([FromBody] AddDeviceDTO device)
        {
            try
            {
                var deviceResult = _deviceService.AddDevice(device);

                return Ok(deviceResult);
            }
            catch (DeviceException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("GetDeviceById")]
        [Authorize(Roles = "admin, client")]
        public IActionResult GetDeviceById(int id)
        {
            try
            {
                var device = _deviceService.GetDeviceById(id);
                return Ok(device);
            }
            catch(DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateDevice")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateDevice([FromBody] UpdateDeviceDTO device)
        {
            try
            {
                _deviceService.UpdateDevice(device);
                return Ok(device);
            }
            catch(DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAllDevicesByUserId")]
        [Authorize(Roles = "admin, client")]
        public async Task<IActionResult> GetAllDevicesByUserIdAsync(string userId)
        {
            try
            {
                var userDevices = await _deviceService.GetAllDevicesByUserId(userId); 
                return Ok(userDevices);
            }
            catch (DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AllocateUserToDevice")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllocatUserToDevice([FromBody] UserDeviceAllocateDTO userDeviceAllocate)
        {
            try
            {
                await _deviceService.AllocateUserToDevice(userDeviceAllocate);
                return Ok(userDeviceAllocate);
            }
            catch (DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("DeallocateUserToDevice")]
        [Authorize(Roles = "admin")]
        public IActionResult DeallocateUserToDevice([FromBody] DeleteDeviceDTO device)
        {
            try
            {
                _deviceService.DeallocateUserToDevice(device);
                return Ok(device);
            }
            catch (DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAllDevicesWithoutUser")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllDevicesWithoutUser()
        {
            var devices = _deviceService.GetAllDevicesWithoutUser();
            return Ok(devices);    
        }

        [HttpGet("GetAllDevicesWithUserAlloc")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllDevicesWithUserAlloc()
        {
            var devices = _deviceService.GetAllDevicesWithUserAlloc();
            return Ok(devices);
        }

        [HttpDelete("DeleteDevice")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteDevice([FromBody] DeleteDeviceDTO device)
        {
            try
            {
                _deviceService.DeleteDevice(device);
                return Ok();
            }
            catch (DeviceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
