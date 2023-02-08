namespace OnlineEnergyUtilityPlatform.DTOs.Measurement
{
    public class AddMeasurementDTO
    {
        public double Consumption { get; set; }
        public DateTime Timestamp { get; set; }
        public int DeviceId { get; set; }
    }
}
