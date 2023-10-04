using CSD.Common.Attributes;
using CSD.Domain.Dto.Users;
using CSD.Story;
using CSD.Story.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    [HttpPost("page")]
    public Task<PageResult<UserDto>> GetUsersPage([FromBody] GetUsersPageContext context) {
        return _getUsersPageStory.ExecuteAsync(context);
    }

    [HttpPost("{id:long}/scene")]
    public Task SetUserScene([FromRoute] long id, [FromBody] SetUserSceneStoryContext context) {
        context.UserId = id;
        return _setUserSceneStory.ExecuteAsync(context);
    }
}
