namespace OnlineEnergyUtilityPlatform.DTOs.User
{
    public class AuthenticateUserDTO
    {
        public string Id { get; set; }  
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }    
    }
}
