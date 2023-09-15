using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSD.WebApp.Middlewares;

public class AppExceptionMiddleware
{
    private readonly ILogger<AppExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AppExceptionMiddleware(
        ILogger<AppExceptionMiddleware> logger,
        RequestDelegate next,
        IConfiguration configuration) { 
        _logger = logger;
        _configuration = configuration;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch (Exception ex) { 
            switch (ex) {
                case ArgumentException:
                    await HandleExceptionAsync(ex.Message, context, ex, HttpStatusCode.BadRequest);
                    break;

                default:
                    await HandleExceptionAsync("Internal server error", context, ex, HttpStatusCode.InternalServerError);
                    break;
            }
        }
    }

    private Task HandleExceptionAsync(string title, HttpContext context, Exception ex, HttpStatusCode code)
    {
        string message = $"{code}: {ex.Message}";
        _logger.LogError(ex, message, ex.StackTrace);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = _configuration.GetSection("AllowFullError").Get<bool>() ? ex.ToString() : null,
            Status = (int)code,
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(
            problemDetails,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
