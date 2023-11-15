using System.Threading.Tasks;
using CSD.Contracts.Users;

namespace CSD.Blazor.Services;

public interface IUserService
{
    Task<PageResult<UserDto>> GetUsersPageAsync();
}
