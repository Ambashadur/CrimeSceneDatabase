using System.Threading.Tasks;
using CSD.Contracts.Users;

namespace CSD.Common;

public interface IUserTokenService
{
    Task SetAsync(string userToken, UserDto userDto);

    Task DeleteAsync(string userToken);

    Task<UserDto?> GetAsync(string userToken);
}
