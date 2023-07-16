using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Common.Settings;

public class AppSettings : IJwtSettings
{
    public string Issuer { get; }

    public string Audience { get; }

    public string Key { get; }

    public TimeSpan JwtLifeTime { get; }

    public AppSettings(IConfiguration configuration) {
        Issuer = configuration["JWT:Issuer"] ?? string.Empty;
        Audience = configuration["JWT:Audience"] ?? string.Empty;
        Key = configuration["JWT:Key"] ?? string.Empty;
        JwtLifeTime = configuration.GetValue("JWT:Lifetime", TimeSpan.FromHours(24));
    }

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));
}
