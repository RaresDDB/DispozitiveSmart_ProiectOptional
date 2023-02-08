using OnlineEnergyUtilityPlatform.Models;

namespace OnlineEnergyUtilityPlatform.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        List<Measurement> GetAllMeasurement();
        Measurement? GetMeasurementById(int measurementId);  
        void AddMeasurement(Measurement measurement);
        void UpdateMeasurement(Measurement measurement);    
        void DeleteMeasurement(Measurement measurement);
        List<Measurement> GetAllMeasurementByDeviceByCurrentMeasurement(Measurement measurement);
        List<Measurement> GetAllMeasurementsByDayByDevice(int id, DateTime timeStamp);
        List<Measurement> GetAllMeasurementsByDevice(int id);
    }
}
