using System;
using Microsoft.IdentityModel.Tokens;

namespace Common.Settings;

public interface IJwtSettings
{
    string Issuer { get; }

    string Audience { get; }

    string Key { get; }

    TimeSpan JwtLifeTime { get; }

    SymmetricSecurityKey GetSymmetricSecurityKey();
}
