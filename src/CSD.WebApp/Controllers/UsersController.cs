using System.Threading.Tasks;
using CSD.Common.Attributes;
using CSD.Contracts;
using CSD.Contracts.Users;
using CSD.Domain.Enums;
using CSD.Story;
using CSD.Story.Users;
using Microsoft.AspNetCore.Mvc;

namespace CSD.WebApp.Controllers;

[ApiController]
[Authorization]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly IStory<PageResult<UserDto>, GetUsersPageContext> _getUsersPageStory;
    private readonly IStory<SetUserSceneStoryContext> _setUserSceneStory;

    public UsersController(
        IStory<PageResult<UserDto>, GetUsersPageContext> getUsersPageStory,
        IStory<SetUserSceneStoryContext> setUserSceneStory) {
        _getUsersPageStory = getUsersPageStory;
        _setUserSceneStory = setUserSceneStory;
    }

    [HttpGet("page")]
    [Authorization(Role = UserRole.Admin)]
    public Task<PageResult<UserDto>> GetUsersPage([FromQuery] GetUsersPageContext context) {
        return _getUsersPageStory.ExecuteAsync(context);
    }

    [HttpPost("{id:long}/scene")]
    [Authorization(Role = UserRole.Admin)]
    public Task SetUserScene([FromRoute] long id, [FromBody] SetUserSceneStoryContext context) {
        context.UserId = id;
        return _setUserSceneStory.ExecuteAsync(context);
    }
}
