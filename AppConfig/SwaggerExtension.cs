using Microsoft.OpenApi.Models;

namespace FinBookeAPI.AppConfig;

public static class SwaggerExtension
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    /// <summary>
    /// This method adds <c>Swagger</c> to this application with some custom configurations.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <returns>The modified <c>IServiceCollection</c></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Define a security scheme
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization. This will add a JWT token (Bearer scheme) to the header. Example: 'Authorization: Bearer [TOKEN]'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                }
            );
            // Define which scheme is required for accessing operations
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        // Scheme that should be used
                        new OpenApiSecurityScheme
                        {
                            // Get a reference to an existing scheme
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });
        return services;
    }

    /// <summary>
    /// This method activates <c>Swagger</c> for the current running instances.
    /// </summary>
    /// <param name="app">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
    /// <returns>The modified <c>IApplicationBuilder</c></returns>
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "FinBooKe Web-API V1");
        });
        return app;
    }
}
