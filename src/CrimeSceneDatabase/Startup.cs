using Common;
using Common.Impl;
using Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Story.User;
using Story.User.Impl;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace CrimeSceneDatabase;

public class Startup
{
    public IWebHostEnvironment WebHostEnvironment { get; }

    public IConfiguration Configuration { get; }

    private const string APP_VERSION = "0.1";

    public Startup(IWebHostEnvironment env, IConfiguration config) {
        WebHostEnvironment = env;
        Configuration = config;
    }

    public void ConfigureServices(IServiceCollection services) {
        services.AddCors();

        services
            .AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddResponseCompression(x => x.EnableForHttps = true);
        services.AddMvc();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c => {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                  "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                  "Example: \"Bearer 12345abcdef\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration["JWT:Key"])),
                        ValidateIssuerSigningKey = true
                    };
                });
        });

        services.AddHttpContextAccessor();

        services.AddSingleton<IJwtSettings, AppSettings>();
        services.AddSingleton<IUserTokenService, UserTokenService>();

        services.AddScoped<IJwtHandler, JwtHandler>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IUserStories, UserStories>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:5010")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        }

        if (env.EnvironmentName != "Production") {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        CultureInfo[] supportedCultures = { new("en"), new("ru") };

        app.UseRequestLocalization(new RequestLocalizationOptions {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseRouting();

        app.UseSwagger();
        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
