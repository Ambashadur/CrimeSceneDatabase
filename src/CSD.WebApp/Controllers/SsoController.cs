using System.Threading.Tasks;
using CSD.Common;
using CSD.Common.Attributes;
using CSD.Common.DataAccess;
using CSD.Common.Helpers;
using CSD.Domain.Dto.Users;
using CSD.Story;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SsoController : ControllerBase
{
    private readonly IStory<string, LoginDto> _userLoginStory;
    private readonly IStory<UserDto, RegisterUserDto> _registerUserStory;
    private readonly CsdContext _context;
    private readonly IAuthService _authService;
    private readonly IUserContext _userContext;

    public SsoController(
        IStory<string, LoginDto> userStories,
        IStory<UserDto, RegisterUserDto> registerUserStory,
        IAuthService authService,
        IUserContext userContext,
        CsdContext csdContext) {
        _userLoginStory = userStories;
        _registerUserStory = registerUserStory;
        _authService = authService;
        _userContext = userContext;
        _context = csdContext;
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
    public Task<UserDto> Register([FromBody] RegisterUserDto registerUserDto) {
        return _registerUserStory.ExecuteAsync(registerUserDto);
    }

    [HttpGet("about")]
    [Authorization]
    public async Task<UserDto> GetAboutInfo() {
        var userDto = _userContext.CurrentUser;
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userDto.Id);
        var scene = await _context.Scenes.FirstOrDefaultAsync(x => x.Id == user.SceneId);

        userDto.SceneId = user?.SceneId;
        userDto.SceneName= scene?.Name;
        userDto.SceneFileLink = scene is null
            ? string.Empty
            : string.Format("api/scenes/{0}/preview?hash={1}", scene.Id, HashHelper.ComputeHash(scene.FileName));

        return userDto;
    }
}
