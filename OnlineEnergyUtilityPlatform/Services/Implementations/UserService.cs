using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineEnergyUtilityPlatform.DTOs.Device;
using OnlineEnergyUtilityPlatform.DTOs.User;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Models;
using OnlineEnergyUtilityPlatform.Repositories.Interfaces;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineEnergyUtilityPlatform.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDeviceService _deviceService;
        private readonly IDeviceRepository _deviceRepository;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IDeviceService deviceService, IDeviceRepository deviceRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _deviceService = deviceService;
            _deviceRepository = deviceRepository;
        }

        public async Task<GetUserDTO> Register(AddUserDTO userToValidate)
        {
            if (!IsValidUsername(userToValidate.Username))
            {
                throw new UserException("Username minimum 4 characters requierd!");
            }

            var usernameAlreadyExist = await _userManager.FindByNameAsync(userToValidate.Username) != null;

            if (usernameAlreadyExist)
            {
                throw new UserException("Username already exist!");
            }

            if (!IsValidEmail(userToValidate.Email))
            {
                throw new UserException("Bad email format!");
            }

            var emailAlreadyExist = await _userManager.FindByEmailAsync(userToValidate.Email) != null;

            if (emailAlreadyExist)
            {
                throw new UserException("Email already exist!");
            }

            var user = new User { UserName = userToValidate.Username, Email = userToValidate.Email };
            var userCreated = await _userManager.CreateAsync(user, userToValidate.Password);

            if (!userCreated.Succeeded)
            {
                var errorMsg = userCreated.Errors.First().Description;
                throw new UserException(errorMsg);
            }

            var clientRole = _roleManager.Roles.ToList().FirstOrDefault(x => x.Name.Equals("client")).Name;
            if (string.IsNullOrEmpty(clientRole))
            {
                clientRole = string.Empty;
            }

            var roleExist = await _roleManager.RoleExistsAsync(clientRole);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new Role { Name = clientRole });
            }

            var result = await _userManager.AddToRoleAsync(user, clientRole);
            var userToReturn = await _userManager.FindByNameAsync(user.UserName);
            var userDTO = new GetUserDTO { id = userToReturn.Id, username = userToReturn.UserName, email = userToReturn.Email };

            return userDTO;
        }

        public async Task<AuthenticateUserDTO> Login(LoginDTO user)
        {
            var attemptUser = await _userManager.FindByNameAsync(user.Username);

            if (attemptUser == null)
            {
                throw new UserException("The user with specified username not found!");
            }

            var authenticateUserResult = await _signInManager.PasswordSignInAsync(attemptUser, user.Password, false, false);

            if (!authenticateUserResult.Succeeded)
            {
                throw new UserException("The password is incorrect!");
            }

            var secretKey = "This is a sample secret key - please don't use in production environment.'";
            var key = Encoding.ASCII.GetBytes(secretKey);
            var userRoles = await _userManager.GetRolesAsync(attemptUser);
            var firstRole = userRoles.First();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, firstRole),
                new Claim("UserId", attemptUser.Id.ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var userRole = _userManager.GetRolesAsync(attemptUser).Result.FirstOrDefault();
            var authenticateUser = new AuthenticateUserDTO { Id = attemptUser.Id, UserName = attemptUser.UserName, Email = attemptUser.Email, Token = jwtToken, Role = userRole };
            await _userManager.SetAuthenticationTokenAsync(attemptUser, "client platform", "auth token", jwtToken);

            return authenticateUser;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public List<GetUserDTO> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var usersDTO = new List<GetUserDTO>();

            foreach (User user in users)
            {
                usersDTO.Add(new GetUserDTO { id = user.Id, username = user.UserName, email = user.Email });
            }

            return usersDTO;
        }

        public async Task<GetUserDTO?> GetUserById(string id)
        {
            var existUser = await _userManager.FindByIdAsync(id);

            if (existUser == null)
            {
                throw new UserException("The user doesn't exist!");
            }

            var currentUser = new GetUserDTO { id = existUser.Id, email = existUser.Email, username = existUser.UserName };

            return currentUser;
        }

        public async Task<GetUserDTO> UpdateUser(UpdateUserDTO updatedUser)
        {
            var currentUser = await _userManager.FindByIdAsync(updatedUser.Id);

            if (currentUser == null)
            {
                throw new UserException("The user doesn't exist!");
            }

            if (!IsValidUsername(updatedUser.Username))
            {
                throw new UserException("Username minimum 4 characters requierd!");
            }

            var usernameCount = _userManager.Users.Where(x => x.UserName.Equals(updatedUser.Username)).Count();

            if (usernameCount > 1)
            {
                throw new UserException("Username already exist!");
            }
            else
            {
                if (usernameCount == 1)
                {
                    var identicalUser = await _userManager.FindByNameAsync(updatedUser.Username);
                    if (!identicalUser.Id.Equals(currentUser.Id))
                    {
                        throw new UserException("Username already exist!");
                    }
                }
            }

            if (!IsValidEmail(updatedUser.Email))
            {
                throw new UserException("Bad email format!");
            }

            var emailCount = _userManager.Users.Where(x => x.Email.Equals(updatedUser.Email)).Count();

            if (emailCount > 1)
            {
                throw new UserException("Email already exist!");
            }
            else
            {
                if (emailCount == 1)
                {
                    var identicalUser = await _userManager.FindByEmailAsync(updatedUser.Email);
                    if (!identicalUser.Id.Equals(currentUser.Id))
                    {
                        throw new UserException("Email already exist!");
                    }
                }
            }

            if (currentUser.UserName.Equals(updatedUser.Username) && currentUser.Email.Equals(updatedUser.Email))
            {
                throw new UserException("The user values ​​have not changed!");
            }

            currentUser.UserName = updatedUser.Username;
            currentUser.Email = updatedUser.Email;
            var result = await _userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                throw new UserException("The user cannot be updated!");
            }

            return new GetUserDTO { id = currentUser.Id, username = currentUser.UserName, email = currentUser.Email };
        }

        public async Task<GetUserDTO> RemoveUser(DeleteUserDTO user)
        {
            var existUser = await _userManager.FindByIdAsync(user.Id);

            if (existUser == null)
            {
                throw new UserException("The user doesn't exist!");
            }

            var deleteRole = await _userManager.GetRolesAsync(existUser);
            await _userManager.RemoveFromRoleAsync(existUser, deleteRole.FirstOrDefault(x => x.Equals("client")));
            var devices = _deviceRepository.GetAllDevicesByUserId(existUser.Id);

            foreach (var device in devices)
            {
                var deviceDTO = new DeleteDeviceDTO { Id = device.Id };
                _deviceService.DeallocateUserToDevice(deviceDTO);
            }

            var result = await _userManager.DeleteAsync(existUser);

            if (!result.Succeeded)
            {
                throw new UserException("The user cannot be deleted!");
            }

            return new GetUserDTO { id = existUser.Id, username = existUser.UserName, email = existUser.Email };
        }

        private static bool IsValidUsername(string username)
        {
            if (username.Length < 4)
                return false;

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

    }
}
