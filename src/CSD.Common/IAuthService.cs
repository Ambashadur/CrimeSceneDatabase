using System.Threading.Tasks;
using CSD.Contracts.Users;

namespace CSD.Common;

public interface IAuthService
{
    Task<string> SignInAsync(UserDto user);

    Task SignOutAsync();
}
