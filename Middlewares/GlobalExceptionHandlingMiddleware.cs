using System.Net;
using System.Text.Json;
using BlogAPI.Exceptions;

namespace BlogAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    IHostEnvironment hostEnvironment,
    ILogger<GlobalExceptionHandlingMiddleware> logger
)
{
    private readonly RequestDelegate _next = next;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        // default values
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var error = "Internal Server Error";
        var message = "An unexpected error occurred. Please try again";

        switch (exception)
        {
            case TokenNotFoundException tokenNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                error = tokenNotFoundException.Message;
                message = "Something went wrong";
                break;
            case UserNotFoundException userNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                error = userNotFoundException.Message;
                message = "Something went wrong";
                break;
            case VerificationTokenExpiredException verificationTokenExpiredException:
                statusCode = (int)HttpStatusCode.NotFound;
                error = verificationTokenExpiredException.Message;
                message = "This verification token has expired. Please request for a new one";
                break;
        }

        var errorResponse = new
        {
            Status = statusCode,
            Message = message,
            Errors = new List<string> { error },
        };

        if (_hostEnvironment.IsDevelopment())
        {
            if (exception.InnerException != null)
            {
                _logger.LogError(exception.InnerException.Message);
            }
            else
            {
                _logger.LogError(exception.Message);
            }
        }

        // serialize to JSON
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(jsonResponse);
    }
}