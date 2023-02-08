using OnlineEnergyUtilityPlatform.Data;
using OnlineEnergyUtilityPlatform.Models;
using OnlineEnergyUtilityPlatform.Repositories.Interfaces;

namespace OnlineEnergyUtilityPlatform.Repositories.Implementations
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly EnergyPlatformDbContext _energyPlatformDbContext;

        public MeasurementRepository(EnergyPlatformDbContext energyPlatformDbContext)
        {
            _energyPlatformDbContext = energyPlatformDbContext;
        }

        public void AddMeasurement(Measurement measurement)
        {
            _energyPlatformDbContext.Measurement.Add(measurement);
            _energyPlatformDbContext.SaveChanges();
        }

        public void DeleteMeasurement(Measurement measurement)
        {
            _energyPlatformDbContext.Measurement.Remove(measurement);
            _energyPlatformDbContext.SaveChanges();
        }

        public List<Measurement> GetAllMeasurement()
        {
            return _energyPlatformDbContext.Measurement.ToList();
        }

        public List<Measurement> GetAllMeasurementByDeviceByCurrentMeasurement(Measurement measurement)
        {
            return _energyPlatformDbContext.Measurement.Where(x =>
            x.DeviceId == measurement.DeviceId &&
            x.TimeStamp.Year.Equals(measurement.TimeStamp.Year) &&
            x.TimeStamp.Month.Equals(measurement.TimeStamp.Month) &&
            x.TimeStamp.Day.Equals(measurement.TimeStamp.Day) &&
            x.TimeStamp.Hour.Equals(measurement.TimeStamp.Hour)).ToList();
        }

        public List<Measurement> GetAllMeasurementsByDayByDevice(int id, DateTime timeStamp)
        {
            return _energyPlatformDbContext.Measurement.Where(x => x.DeviceId == id && x.TimeStamp.Day.Equals(timeStamp.Day)).ToList();
        }

        public List<Measurement> GetAllMeasurementsByDevice(int id)
        {
            return _energyPlatformDbContext.Measurement.Where(x => x.DeviceId == id).ToList();
        }

        public Measurement? GetMeasurementById(int measurementId)
        {
            return _energyPlatformDbContext.Measurement.SingleOrDefault(x => x.Id == measurementId);
        }

        public void UpdateMeasurement(Measurement measurement)
        {
            _energyPlatformDbContext.Measurement.Update(measurement);
            _energyPlatformDbContext.SaveChanges();
        }
    }
}
