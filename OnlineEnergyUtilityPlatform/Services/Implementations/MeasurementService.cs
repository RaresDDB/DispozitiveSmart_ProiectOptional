using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OnlineEnergyUtilityPlatform.DTOs.Measurement;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Models;
using OnlineEnergyUtilityPlatform.Repositories.Interfaces;
using OnlineEnergyUtilityPlatform.Services.Interfaces;

namespace OnlineEnergyUtilityPlatform.Services.Implementations
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly IDeviceRepository _deviceRepository;

        public MeasurementService(IMeasurementRepository measurementRepository, IDeviceRepository deviceRepository)
        {
            _measurementRepository = measurementRepository;
            _deviceRepository = deviceRepository;
        }

        public async Task AddMeasurement(AddMeasurementDTO measurement)
        {
            if(measurement.Consumption < 0)
            {
                throw new MeasurementException("The consumption of device must be a positive number!");
            }

            if(measurement.Timestamp == DateTime.MinValue)
            {
                throw new MeasurementException("Time stamp is required!");
            }

            if(string.IsNullOrEmpty(measurement.DeviceId.ToString()))
            {
                throw new MeasurementException("Device allocation is required!");
            }

            _measurementRepository.AddMeasurement(new Measurement
            {
                Consumption = measurement.Consumption,
                TimeStamp = measurement.Timestamp,
                DeviceId = measurement.DeviceId,
                Device = _deviceRepository.GetDeviceById(measurement.DeviceId)
            });

            var deviceAllMeasurements = GetAllMeasurementByDayByDevice(measurement.DeviceId, measurement.Timestamp);
            var hourlyConsumption = GetCurrentHourlyConsumptionValue(deviceAllMeasurements, measurement);
            var currentDevice = _deviceRepository.GetDeviceById(measurement.DeviceId);
            if(currentDevice == null)
            {
                throw new MeasurementException("The device doesn't exist!");
            }
        }

        public void DeleteMeasurement(DeleteMeasurementDTO measurement)
        {
            var deleteMeasurement = _measurementRepository.GetMeasurementById(measurement.Id);

            if(deleteMeasurement == null)
            {
                throw new MeasurementException("The measurement doesn't exist!");
            }

            _measurementRepository.DeleteMeasurement(deleteMeasurement);
        }

        public List<GetMeasurementDTO> GetAllMeasurement()
        {
            var measurementDTO = new List<GetMeasurementDTO>();

            foreach(var measurement in _measurementRepository.GetAllMeasurement())
            {
                measurementDTO.Add(new GetMeasurementDTO
                {
                    Consumption = measurement.Consumption,
                    Timestamp = measurement.TimeStamp,
                    DeviceId = measurement.DeviceId
                });
            }

            return measurementDTO;
        }

        public List<GetMeasurementDTO> GetAllMeasurementByDeviceByCurrentMeasurement(AddMeasurementDTO measurement)
        {
            var measurementDTO = new List<GetMeasurementDTO>();
            Measurement concreteMeasurement = new Measurement
            {
                Consumption = measurement.Consumption,
                TimeStamp = measurement.Timestamp,
                DeviceId = measurement.DeviceId,
                Device = _deviceRepository.GetDeviceById(measurement.DeviceId)
            };

            foreach(var localMeasurement in _measurementRepository.GetAllMeasurementByDeviceByCurrentMeasurement(concreteMeasurement))
            {
                measurementDTO.Add(new GetMeasurementDTO
                {
                    Consumption = localMeasurement.Consumption,
                    Timestamp = localMeasurement.TimeStamp,
                    DeviceId = localMeasurement.DeviceId
                });
            }

            return measurementDTO;
        }

        public List<GetMeasurementDTO> GetAllMeasurementByDayByDevice(int deviceId, DateTime timeStamp)
        {
            if (timeStamp == DateTime.MinValue)
            {
                throw new MeasurementException("Time stamp is required!");
            }

            var device = _deviceRepository.GetDeviceById(deviceId);
            if(device == null)
            {
                throw new MeasurementException("The measurement doesn't exit!");
            }

            var measurements = _measurementRepository.GetAllMeasurementsByDayByDevice(deviceId, timeStamp);
            var sortedMeasurements = measurements.OrderBy(x => x.TimeStamp);
            var deviceMeasurements = new List<GetMeasurementDTO>();

            foreach(var measurement in sortedMeasurements)
            {
                deviceMeasurements.Add(new GetMeasurementDTO
                {
                    Consumption = measurement.Consumption,
                    Timestamp = measurement.TimeStamp,
                    DeviceId = measurement.DeviceId
                });
            }

            return deviceMeasurements;
        }

        public List<GetMeasurementDTO> GetAllMeasurementByDevice(int id)
        {
            var measurements = _measurementRepository.GetAllMeasurementsByDevice(id);

            var measurementsDTO = new List<GetMeasurementDTO>();

            foreach(var measurement in measurements)
            {
                measurementsDTO.Add(new GetMeasurementDTO
                {
                    Consumption = measurement.Consumption,
                    Timestamp = measurement.TimeStamp,
                    DeviceId = measurement.DeviceId,
                });
            }

            return measurementsDTO;
        }

        public GetMeasurementDTO GetMeasurementById(int measurementId)
        {
            var currentMeasurement = _measurementRepository.GetMeasurementById(measurementId);

            if(currentMeasurement == null)
            {
                throw new MeasurementException("The measurement doesn't exit!");
            }

            var measurementById = new GetMeasurementDTO
            {
                Consumption = currentMeasurement.Consumption,
                Timestamp = currentMeasurement.TimeStamp,
                DeviceId = currentMeasurement.DeviceId
            };

            return measurementById;
        }

        public void UpdateMeasurement(UpdateMeasurementDTO measurement)
        {
            var currentMeasurement = _measurementRepository.GetMeasurementById(measurement.Id);

            if(currentMeasurement == null)
            {
                throw new MeasurementException("The measurement doesn't exit!");
            }

            if (measurement.Consumption <= 0)
            {
                throw new MeasurementException("The consumption of device must be a positive number!");
            }

            if (measurement.Timestamp == DateTime.MinValue)
            {
                throw new MeasurementException("Time stamp is required!");
            }

            if (string.IsNullOrEmpty(measurement.DeviceId.ToString()))
            {
                throw new MeasurementException("Device allocation is required!");
            }

            if(currentMeasurement.Consumption == measurement.Consumption && currentMeasurement.TimeStamp == measurement.Timestamp
                && currentMeasurement.DeviceId == measurement.DeviceId)
            {
                throw new MeasurementException("The measurement values has not changed!");
            }

            currentMeasurement.Consumption = measurement.Consumption;
            currentMeasurement.TimeStamp = measurement.Timestamp;
            currentMeasurement.DeviceId = measurement.DeviceId;
            currentMeasurement.Device = _deviceRepository.GetDeviceById(measurement.DeviceId);

            _measurementRepository.UpdateMeasurement(currentMeasurement);
        }

        private static double GetCurrentHourlyConsumptionValue(List<GetMeasurementDTO> measurements, AddMeasurementDTO currentMeasurement)
        {
            double sum = 0;

            foreach(var measurement in measurements)
            {
                if(currentMeasurement.Timestamp.Hour.Equals(measurement.Timestamp.Hour))
                    sum += measurement.Consumption;
            }

            return sum;
        }

    }
}
