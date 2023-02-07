namespace OnlineEnergyUtilityPlatform.DTOs.Device
{
    public class UpdateDeviceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double MaximumConsumption { get; set; }
    }
}
