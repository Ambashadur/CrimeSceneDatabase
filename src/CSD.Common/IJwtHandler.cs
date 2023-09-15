using System.Security.Claims;
using CSD.Domain.Dto;

namespace CSD.Common;

public interface IJwtHandler
{
    string GenerateJwtToken(UserDto? userDto);

    ClaimsPrincipal GetClaimsPrincipal(UserDto userDto);

    UserDto DecodeJwtToken(string userToken);
}
