using System;
using System.Text;
using System.Text.Json.Serialization;
using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Common.Impl;
using CSD.Common.Settings;
using CSD.Story.User;
using CSD.Story.User.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CSD.WebApp;

public class Startup
{
    public IWebHostEnvironment WebHostEnvironment { get; }

    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment env, IConfiguration config) {
        WebHostEnvironment = env;
        Configuration = config;
    }

    public void ConfigureServices(IServiceCollection services) {
        services.AddCors();
        services.AddLogging();
        services.AddHttpLogging(opt => opt.LoggingFields = HttpLoggingFields.All);

        services
            .AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddResponseCompression(x => x.EnableForHttps = true);
        services.AddMvc();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();

        services.AddDbContext<CsdContext>((serviceProvider, options) => {
            var settings = serviceProvider.GetRequiredService<IDbSettings>();

            options.UseNpgsql(settings.ConnectionString);
            options.LogTo(message => Console.WriteLine(message));
        });

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
                            Encoding.ASCII.GetBytes(Configuration["JWT:Key"] ?? string.Empty)),
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

        app.UseRouting();

        app.UseSwagger();
        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseHttpLogging();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
