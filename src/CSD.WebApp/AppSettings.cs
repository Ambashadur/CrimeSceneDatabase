using CSD.Common;
using CSD.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CSD.WebApp;

public class AppSettings : IJwtSettings, IDbSettings
{
    public AppSettings(IConfiguration config) {
        ConnectionString = config.GetConnectionString("CSD") ?? string.Empty;
        Issuer = config["JWT:Issuer"] ?? string.Empty;
        Audience = config["JWT:Audience"] ?? string.Empty;
        Key = config["JWT:Key"] ?? string.Empty;
        JwtLifeTime = config.GetValue<TimeSpan>("JWT:Lifetime");
    }

    public string ConnectionString { get; }

    public string Issuer { get; }

    public string Audience { get; }

    public string Key { get; }

    public TimeSpan JwtLifeTime { get; }

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}
