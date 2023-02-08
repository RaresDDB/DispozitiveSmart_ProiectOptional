namespace OnlineEnergyUtilityPlatform.DTOs.Measurement
{
    public class UpdateMeasurementDTO
    {
        public int Id { get; set; }
        public double Consumption { get; set; }
        public DateTime Timestamp { get; set; }
        public int DeviceId { get; set; }
    }
}
