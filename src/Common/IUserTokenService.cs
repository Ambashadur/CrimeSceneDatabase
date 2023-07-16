using System.Threading.Tasks;
using Domain.Dto;

namespace Common;

public interface IUserTokenService
{
    Task SetAsync(string userToken, UserDto userDto);

    Task DeleteAsync(string userToken);

    Task<UserDto?> GetAsync(string userToken);
}
