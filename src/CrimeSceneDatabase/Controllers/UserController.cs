using System.Threading.Tasks;
using Common;
using Common.Attributes;
using Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Story.User;

namespace CrimeSceneDatabase.Controllers;

[ApiController]
[Route("sso/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserStories _userStories;
    private readonly IAuthService _authService;

    public UserController(IUserStories userStories, IAuthService authService) {
        _userStories = userStories;
        _authService = authService;
    }

    [HttpPost("login")]
    public Task<string> Login([FromBody] LoginDto loginDto) {
        return _userStories.LoginAsync(loginDto);
    }

    [HttpPost("logout")]
    [Authorization]
    public Task Logout() {
        return _authService.SignOutAsync();
    }

    [HttpGet("auth-test")]
    [Authorization]
    public Task Test() {
        return Task.CompletedTask;
    }
}
