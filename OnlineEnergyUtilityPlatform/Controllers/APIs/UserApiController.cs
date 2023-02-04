using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnergyUtilityPlatform.DTOs.User;
using OnlineEnergyUtilityPlatform.Exceptions;
using OnlineEnergyUtilityPlatform.Services.Interfaces;
using System.Security.Claims;
using System.Security.Principal;

namespace OnlineEnergyUtilityPlatform.Controllers.APIs
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "admin")]
        public List<GetUserDTO> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AddUserDTO user)
        {
            try
            {
                var userResult = await _userService.Register(user);
                return Ok(userResult);
            }
            catch(UserException e)
            {
                return BadRequest(e.Message);
            }
             
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            try
            {
                var authenticateUser =  await _userService.Login(user);

                if(authenticateUser.Token != null)
                {
                    HttpContext.Session.SetString("JWToken", authenticateUser.Token);
                    bool islogin = User.Identity.IsAuthenticated;
                }

                return Ok(authenticateUser);
            }
            catch(UserException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Logout")]
        [Authorize(Roles = "admin, client")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            HttpContext.Session.SetString("JWToken", "");
            return Ok();
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var currentUser = await _userService.GetUserById(id);
                return Ok(currentUser);
            }
            catch(UserException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO user)
        {
            try
            {
                var userResult = await _userService.UpdateUser(user);
                return Ok(userResult);
            }
            catch(UserException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO user)
        {
            try
            {
                var resultUser = await _userService.RemoveUser(user);
                return Ok(resultUser);
            }
            catch (UserException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
