using CSD.Common;
using CSD.Common.Attributes;
using CSD.Domain.Dto.Users;
using CSD.Story;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("sso/[controller]")]
public class UserController : ControllerBase
{
    private readonly IStory<string, LoginDto> _userLoginStory;
    private readonly IStory<UserDto, RegisterUserDto> _registerUserStory;
    private readonly IAuthService _authService;

    public UserController(
        IStory<string, LoginDto> userStories,
        IStory<UserDto, RegisterUserDto> registerUserStory,
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
