using CSD.Common.DataAccess;
using CSD.Domain.Dto.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Users;

public class GetUsersPageStory : IStory<PageResult<UserDto>, GetUsersPageContext>
{
    private readonly CsdContext _dbContext;

    public GetUsersPageStory(CsdContext dbContext) {
        _dbContext = dbContext;
    }

    public Task<PageResult<UserDto>> ExecuteAsync(GetUsersPageContext context) {
        if (context.Page < 1) throw new ArgumentException("Page must be greater or equal than 1!");

        var users = (from user in _dbContext.Users
                    join scene in _dbContext.Scenes
                        on user.SceneId equals scene.Id into scenes
                    from scene in scenes.DefaultIfEmpty()
                    where user.Role == context.Role
                    select new UserDto {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PaternalName = user.PaternalName,
                        Login = user.Login,
                        Role = user.Role,
                        SceneId = user.SceneId,
                        SceneName = scene.Name
                    }).Skip((context.Page - 1) * context.Count).Take(context.Count);

        return Task.FromResult(new PageResult<UserDto> {
            Page = context.Page,
            Count = users.Count(),
            TotalCount = _dbContext.Users.Where(user => user.Role == context.Role).Count(),
            Data = users
        });
    }
}
