using OnlineEnergyUtilityPlatform.DTOs.User;

namespace OnlineEnergyUtilityPlatform.Services.Interfaces
{
    public interface IUserService
    {
        List<GetUserDTO> GetAllUsers();
        Task<GetUserDTO> Register(AddUserDTO user);
        Task<AuthenticateUserDTO> Login(LoginDTO user);
        Task Logout();
        Task<GetUserDTO> UpdateUser(UpdateUserDTO user);
        Task<GetUserDTO> RemoveUser(DeleteUserDTO user);
        Task<GetUserDTO?> GetUserById(string id);
    }
}
