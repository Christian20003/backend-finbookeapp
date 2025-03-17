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
        switch (exception.Code)
        {
            case ErrorCodes.INVALID_CREDENTIALS:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var body = new ErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = "Invalid username or password",
                    Code = exception.Code,
                };
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.INVALID_TOKEN:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var body = new ErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = "The provided refresh token is invalid",
                    Code = exception.Code,
                };
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.ACCESS_EXPIRED:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var body = new ErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = "The refresh token has expired",
                    Code = exception.Code,
                };
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            case ErrorCodes.ACCESS_LOCKED:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Locked;
                var body = new ErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = "You are currently locked out for further login attempts",
                    Code = exception.Code,
                };
                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }
            default:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var body = new ErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = "A server-side operation failed",
                };
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
            Status = context.Response.StatusCode,
            Message = "A server-side operation failed",
        };
        return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
    }
}
