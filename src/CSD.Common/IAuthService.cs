using System.Threading.Tasks;
using CSD.Domain.Dto.Users;

namespace CSD.Common;

public interface IAuthService
{
    Task<string> SignInAsync(UserDto user);

    Task SignOutAsync();
}
