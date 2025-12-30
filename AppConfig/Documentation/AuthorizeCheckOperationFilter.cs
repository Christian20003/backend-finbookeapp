using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinBookeAPI.AppConfig.Documentation;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize =
            context.MethodInfo.DeclaringType != null
            && (
                context
                    .MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any()
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            );

        var hasAllowAnonymous =
            context.MethodInfo.DeclaringType != null
            && (
                context
                    .MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any()
                || context
                    .MethodInfo.GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any()
            );

        if (!hasAuthorize && hasAllowAnonymous)
        {
            operation.Responses?.TryAdd(
                "401",
                new OpenApiResponse { Description = "Unauthorized" }
            );

            operation.Responses?.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer")] = [],
                },
            ];
        }
    }
}
