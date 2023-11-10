using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Common.Files;
using CSD.Common.Files.Impl;
using CSD.Common.Impl;
using CSD.Common.Settings;
using CSD.Common.VoiceRecognition;
using CSD.Common.VoiceRecognition.Impl;
using CSD.Domain.Dto.Comments;
using CSD.Domain.Dto.Scenes;
using CSD.Domain.Dto.Users;
using CSD.Story;
using CSD.Story.Comments;
using CSD.Story.Scenes;
using CSD.Story.Sso;
using CSD.Story.Users;
using CSD.WebApp.Middlewares;
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
using System;
using System.Text;
using System.Text.Json.Serialization;

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

            options.UseSqlite(settings.ConnectionString);
            options.LogTo(message => Console.WriteLine(message));
        });

        services.AddAuthorization();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"] ?? string.Empty)),
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddSwaggerGen(c => {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                  "Enter your token in the text input below.\r\n\r\n"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        In = ParameterLocation.Header,
                        Name = "Bearer"
                    },
                    new string[] { }
                }
            });
        });

        services.AddHttpContextAccessor();

        services.AddSingleton<IDbSettings, AppSettings>();
        services.AddSingleton<IJwtSettings, AppSettings>();

        services.AddSingleton<IUserTokenService, UserTokenService>();
        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddSingleton<IFileStorage, FileStorage>();
        services.AddSingleton<IVoiceRecognitionService, VoiceRecognitionService>();

        services.AddScoped<IJwtHandler, JwtHandler>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IStory<string, LoginDto>, UserLoginStory>();
        services.AddTransient<IStory<UserDto, RegisterUserDto>, RegisterUserStory>();
        services.AddTransient<IStory<SceneDto, CreateSceneStoryContext>, CreateSceneStory>();
        services.AddTransient<IStory<PageResult<SceneDto>, GetPageContext>, GetPageScenesStory>();
        services.AddTransient<IStory<MediaResult, GetSceneStoryContext>, GetSceneStory>();
        services.AddTransient<IStory<MediaResult, GetAudioFromCommentStoryContext>, GetAudioFromCommentStory>();
        services.AddTransient<IStory<PageResult<CommentDto>, GetCommentsPageContext>, GetCommentsPageStory>();
        services.AddTransient<IStory<MediaResult, GetPhotoFromCommentStoryContext>, GetPhotoFromCommentStory>();
        services.AddTransient<IStory<PageResult<UserDto>, GetUsersPageContext>, GetUsersPageStory>();
        services.AddTransient<IStory<SetUserSceneStoryContext>, SetUserSceneStory>();
        services.AddTransient<IStory<CreateCommentStoryContext>, CreateCommentStory>();
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

        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseHttpLogging();
        app.UseMiddleware<AppExceptionMiddleware>();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
