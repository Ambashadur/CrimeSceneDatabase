using System.Security.Claims;
using CSD.Contracts.Users;

namespace CSD.Common;

public interface IJwtHandler
{
    string GenerateJwtToken(UserDto? userDto);

    ClaimsPrincipal GetClaimsPrincipal(UserDto userDto);

    UserDto DecodeJwtToken(string userToken);
}
