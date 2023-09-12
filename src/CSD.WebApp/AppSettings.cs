using CSD.Common;
using Microsoft.Extensions.Configuration;

namespace CSD.WebApp;

public class AppSettings : IDbSettings
{
    public AppSettings(IConfiguration config) {
        ConnectionString = config.GetConnectionString("CSD") ?? string.Empty;
    }

    public string ConnectionString { get; }
}
