namespace OnlineEnergyUtilityPlatform.DTOs.Device
{
    public class GetDeviceDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public double maximumconsumption { get; set; }
        public string? userid { get; set; }
        public string? username { get; set; }
    }
}
