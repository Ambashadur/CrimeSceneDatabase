using System.Threading.Tasks;
using Common.Settings;
using Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace Common.Impl;

public class AuthService : IAuthService
{
    private const string TOKEN_COOKIE_NAME = "auth_token";

    private readonly IJwtHandler _jwtHandler;
    private readonly IJwtSettings _jwtSettings;
    private readonly IUserTokenService _userTokenService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(
        IUserTokenService userTokenService,
        IJwtSettings jwtSettings,
        IJwtHandler jwtHandler,
        IHttpContextAccessor contextAccessor) {
        _userTokenService = userTokenService;
        _jwtSettings = jwtSettings;
        _jwtHandler = jwtHandler;
        _contextAccessor = contextAccessor;
    }

    public async Task<string> SignInAsync(UserDto userDto) {
        var userToken = _jwtHandler.GenerateJwtToken(userDto);

        await _userTokenService.SetAsync(userToken, userDto);

        var options = new CookieOptions() { MaxAge = _jwtSettings.JwtLifeTime };
        _contextAccessor.HttpContext.Response.Cookies.Append(TOKEN_COOKIE_NAME, userToken, options);

        return userToken;
    }

    public async Task SignOutAsync() {
        if (_contextAccessor.HttpContext.Request.Cookies.TryGetValue(TOKEN_COOKIE_NAME, out var token)) {
            await _userTokenService.DeleteAsync(token);
        }

        _contextAccessor.HttpContext.Response.Cookies.Delete(TOKEN_COOKIE_NAME);
    }
}
