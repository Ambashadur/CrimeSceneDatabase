using System.Threading.Tasks;
using CSD.Contracts;
using CSD.Contracts.Users;

namespace CSD.Blazor.Services;

public interface IUserService
{
    Task<PageResultContract<UserDto>> GetUsersPageAsync();
}
