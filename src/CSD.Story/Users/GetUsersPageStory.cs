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

        var users = _dbContext.Users
            .Where(user => user.Role == context.Role)
            .Skip((context.Page - 1) * context.Count)
            .Take(context.Count)
            .Select(user => new UserDto() {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PaternalName = user.PaternalName,
                Login = user.Login,
                Role = context.Role,
                SceneId = user.SceneId
            });

        return Task.FromResult(new PageResult<UserDto>() {
            Page = context.Page,
            Count = users.Count(),
            Data = users
        });
    }
}
