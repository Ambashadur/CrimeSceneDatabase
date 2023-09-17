using CSD.Domain.Dto.Users;
using CSD.Story;
using CSD.Story.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController :  ControllerBase
{
    private readonly IStory<PageResult<UserDto>, GetUsersPageContext> _getUsersPageStory;

    public UserController(
        IStory<PageResult<UserDto>, GetUsersPageContext> getUsersPageStory) {
        _getUsersPageStory = getUsersPageStory;
    }

    [HttpPost("page")]
    public Task<PageResult<UserDto>> GetUsersPage([FromBody] GetUsersPageContext context) {
        return _getUsersPageStory.ExecuteAsync(context);
    }
}
