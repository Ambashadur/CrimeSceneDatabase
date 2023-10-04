using CSD.Domain.Dto.Users;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Common.Impl;

public class AuthService : IAuthService
{
    private const string HEADER_NAME = "Authorization";

    private readonly IJwtHandler _jwtHandler;
    private readonly IUserTokenService _userTokenService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(
        IUserTokenService userTokenService,
        IJwtHandler jwtHandler,
        IHttpContextAccessor contextAccessor) {
        _userTokenService = userTokenService;
        _jwtHandler = jwtHandler;
        _contextAccessor = contextAccessor;
    }

    public async Task<string> SignInAsync(UserDto userDto) {
        var userToken = _jwtHandler.GenerateJwtToken(userDto);

        await _userTokenService.SetAsync(userToken, userDto);

        return userToken;
    }

    public async Task SignOutAsync() {
        if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(HEADER_NAME, out var headerValue)) {
            await _userTokenService.DeleteAsync(headerValue.ToString().Remove(0, 7));
        }
    }
}
