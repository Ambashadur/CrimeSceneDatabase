using System.Threading.Tasks;
using CSD.Common;
using CSD.Common.Attributes;
using CSD.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using CSD.Story.User;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("sso/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserLoginStory _userStories;
    private readonly IAuthService _authService;

    public UserController(IUserLoginStory userStories, IAuthService authService) {
        _userStories = userStories;
        _authService = authService;
    }

    [HttpPost("login")]
    public Task<string> Login([FromBody] LoginDto loginDto) {
        return _userStories.ExecuteAsync(loginDto);
    }

    [HttpPost("logout")]
    [Authorization]
    public Task Logout() {
        return _authService.SignOutAsync();
    }

    [HttpPost("register")]
    [Authorization]
    public async Task<UserDto> Register(RegisterUserDto registerUserDto) { 

    }
}
