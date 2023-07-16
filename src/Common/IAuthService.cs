using System.Threading.Tasks;
using Domain.Dto;

namespace Common;

public interface IAuthService
{
    Task<string> SignInAsync(UserDto user);

    Task SignOutAsync();
}
