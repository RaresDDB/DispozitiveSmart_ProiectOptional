using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.Measurement;

namespace OnlineEnergyUtilityPlatform.DTOs.DeviceMeasurement
{
    public class DeviceMeasurementsDTO
    {
        public GetDeviceDTO Device { get; set; }    
        public List<GetMeasurementDTO> Measurements { get; set; }
    }
}
