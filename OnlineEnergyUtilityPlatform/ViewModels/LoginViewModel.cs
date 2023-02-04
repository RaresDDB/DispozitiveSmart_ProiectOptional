namespace OnlineEnergyUtilityPlatform.ViewModels
{
    public class LoginViewModel
    {
        public string RegisterUrl { get; set; }

        public LoginViewModel(string registerUrl)
        {
            RegisterUrl = registerUrl;
        }
    }
}
