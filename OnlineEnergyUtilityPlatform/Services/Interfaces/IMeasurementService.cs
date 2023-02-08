using OnlineEnergyUtilityPlatform.DTOs.DeviceMeasurement;
using OnlineEnergyUtilityPlatform.DTOs.Measurement;

namespace OnlineEnergyUtilityPlatform.Services.Interfaces
{
    public interface IMeasurementService
    {
        List<GetMeasurementDTO> GetAllMeasurement();
        GetMeasurementDTO GetMeasurementById(int measurementId);
        Task AddMeasurement(AddMeasurementDTO measurement);
        void UpdateMeasurement(UpdateMeasurementDTO measurement);
        void DeleteMeasurement(DeleteMeasurementDTO measurement);
        List<GetMeasurementDTO> GetAllMeasurementByDeviceByCurrentMeasurement(AddMeasurementDTO measurement);
        List<GetMeasurementDTO> GetAllMeasurementByDayByDevice(int deviceId, DateTime timeStamp);
        List<GetMeasurementDTO> GetAllMeasurementByDevice(int id);
    }
}
