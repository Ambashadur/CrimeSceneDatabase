using System;
using System.Linq;
using System.Threading.Tasks;
using CSD.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CSD.Common.Attributes;

public class AuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public UserRole Role = UserRole.Default;

    private const string AUTH_HEADER = "Authorization";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
        var userToken = context.HttpContext.Request.Headers[AUTH_HEADER].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(userToken)) {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userTokenService = context.HttpContext.RequestServices.GetRequiredService<IUserTokenService>();

        var userDto = await userTokenService.GetAsync(userToken);

        if (userDto is null) {
            context.Result = new UnauthorizedResult();
            return;
        }

        if ((int)userDto.Role < (int)Role) {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();

        if (userContext.CurrentUser is null) {
            try {
                var jwtHandler = context.HttpContext.RequestServices.GetRequiredService<IJwtHandler>();

                userContext.SetCurrentUser(userDto);
                context.HttpContext.User = jwtHandler.GetClaimsPrincipal(userContext.CurrentUser);
            } catch {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
