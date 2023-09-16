using System.Threading.Tasks;
using CSD.Common;
using CSD.Common.Attributes;
using CSD.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using CSD.Story.Users;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("sso/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserLoginStory _userLoginStory;
    private readonly IRegisterUserStory _registerUserStory;
    private readonly IAuthService _authService;

    public UserController(
        IUserLoginStory userStories,
        IRegisterUserStory registerUserStory,
        IAuthService authService) {
        _userLoginStory = userStories;
        _registerUserStory = registerUserStory;
        _authService = authService;
    }

    [HttpPost("login")]
    public Task<string> Login([FromBody] LoginDto loginDto) {
        return _userLoginStory.ExecuteAsync(loginDto);
    }

    [HttpPost("logout")]
    [Authorization]
    public Task Logout() {
        return _authService.SignOutAsync();
    }

    [HttpPost("register")]
    public Task<UserDto> Register(RegisterUserDto registerUserDto) {
        return _registerUserStory.ExecuteAsync(registerUserDto);
    }
}
