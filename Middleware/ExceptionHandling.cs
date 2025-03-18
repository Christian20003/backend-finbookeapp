using System.Net;
using FinBookeAPI.DTO.Error;
using FinBookeAPI.Models.Exceptions;
using Newtonsoft.Json;

namespace FinBookeAPI.Middleware;

public class ExceptionHandling : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (AuthenticationException exception)
        {
            await HandleAuthenticationException(context, exception);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private Task HandleAuthenticationException(
        HttpContext context,
        AuthenticationException exception
    )
    {
        context.Response.ContentType = "application/json";
        var body = new ErrorResponse
        {
            Type = "AuthenticationException",
            Code = exception.Code,
            Instance = context.Request.Path,
        };
        switch (exception.Code)
        {
            case ErrorCodes.INVALID_CREDENTIALS:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                body.Title = "Invalid email or password";
                body.Detail = "Authentication failed due to incorrect email or password";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.UNEXPECTED_STRUCTURE:
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                body.Title = "Missing account property";
                body.Detail = "Authentication failed due to incorrect username";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.INVALID_TOKEN:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                body.Title = "Invalid refresh token";
                body.Detail = "Authentication failed due to an incorrect refresh token";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.EXPIRED_TOKEN:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                body.Title = "Expired refresh token";
                body.Detail = "Authentication failed due to an expired refresh token";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.INVALID_CODE:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                body.Title = "Invalid security code";
                body.Detail = "Reset password failed due to an incorrect security code";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.EXPIRED_CODE:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                body.Title = "Expired security code";
                body.Detail = "Reset password failed due to an expired security code";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.ACCESS_LOCKED:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Locked;
                body.Title = "Access locked (5 minutes)";
                body.Detail =
                    "Authentication failed due to locked access. Too many failed attempts";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            default:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                body.Title = "Server operation failed";
                body.Detail = "Authentication failed due to an unexpected operation failure";
                body.Status = context.Response.StatusCode;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
        }
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var body = new ErrorResponse
        {
            Type = "Exception",
            Title = "Unexpected failure",
            Detail = "Requested operation failed due to an unexpected operation failure",
            Instance = context.Request.Path,
            Status = context.Response.StatusCode,
        };
        return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
    }
}
