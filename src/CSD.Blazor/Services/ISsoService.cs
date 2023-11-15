using System.Threading.Tasks;
using CSD.Contracts.Users;

namespace CSD.Blazor.Services;

public interface ISsoService
{
    Task LoginAsync(LoginDto loginDto);

    Task LogoutAsync();
}
