using System.Reflection;
using Microsoft.OpenApi;

namespace FinBookeAPI.AppConfig.Documentation;

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
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "FinBooKe Web-API", Version = "v1" }
            );
            // Define a security scheme
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization. This will add a JWT token (Bearer scheme) to the header. Example: 'Bearer [TOKEN]'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                }
            );
            // Define which scheme is required for accessing operations
            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", doc)] = [],
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.OperationFilter<AuthorizeCheckOperationFilter>();
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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        });
        return app;
    }
}
