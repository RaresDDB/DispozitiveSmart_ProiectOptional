using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.DTOs.DeviceMeasurement;
using OnlineEnergyUtilityPlatform.DTOs.Measurement;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Services.Interfaces;

namespace OnlineEnergyUtilityPlatform.Controllers.APIs
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MeasurementApiController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementApiController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpGet("GetAllMeasurement")]
        public IActionResult GetAllMeasurement()
        {
            return Ok(_measurementService.GetAllMeasurement());
        }

        [HttpPost("AddMeasurement")]
        public IActionResult AddMeasurement([FromBody] AddMeasurementDTO measurement)
        {
            try
            {
                _measurementService.AddMeasurement(measurement);
                return Ok(measurement);
            }
            catch(MeasurementException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetMeasurementById")]
        public IActionResult GetMeasurementById(int id)
        {
            try
            {
                var measurement = _measurementService.GetMeasurementById(id);
                return Ok(measurement);
            }
            catch (MeasurementException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateMeasurement")]
        public IActionResult UpdateMeasurement([FromBody] UpdateMeasurementDTO measurement)
        {
            try
            {
                _measurementService.UpdateMeasurement(measurement);
                return Ok(measurement);
            }
            catch (MeasurementException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeleteMeasurement")]
        public IActionResult DeleteMeasurement([FromBody] DeleteMeasurementDTO measurement)
        {
            try
            {
                _measurementService.DeleteMeasurement(measurement);
                return Ok();
            }
            catch (MeasurementException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GraphicMeasurements")]
        public IActionResult GetAllMeasurementByDayByDevice(int deviceId, DateTime timeStamp)
        {
            try
            {
                var measuerements = _measurementService.GetAllMeasurementByDayByDevice(deviceId, timeStamp);
                return Ok(measuerements);
            }
            catch (MeasurementException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
