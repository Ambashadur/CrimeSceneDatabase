using System.Security.Claims;
using Domain.Dto;

namespace Common;

public interface IJwtHandler
{
    string GenerateJwtToken(UserDto? userDto);

    ClaimsPrincipal? GetClaimsPrincipal(UserDto? userDto);

    UserDto DecodeJwtToken(string userToken);
}
