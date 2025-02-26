using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.AppConfig;

public static class SettingsExtension
{
    /// <summary>
    /// This method adds the following configuration classes to the dependency injector:
    /// <list type="bullet">
    ///     <item><c><see cref="AuthDatabaseSettings"/></c></item>
    ///     <item><c><see cref="FinancialDataDatabaseSettings"/></c></item>
    ///     <item><c><see cref="JwTSettings"/></c></item>
    /// </list>
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">A set of key/value application configuration properties.</param>
    /// <returns>The modified <c>IServiceCollection</c></returns>
    public static IServiceCollection AddConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add Authentication database informations
        services.Configure<AuthDatabaseSettings>(
            configuration.GetSection(AuthDatabaseSettings.SectionName)
        );
        // Add financial data database informations
        services.Configure<FinancialDataDtabaseSettings>(
            configuration.GetSection(FinancialDataDtabaseSettings.SectionName)
        );
        // Add JWT token settings
        services.Configure<JwTSettings>(configuration.GetSection(JwTSettings.SectionName));

        services.Configure<SmtpServer>(configuration.GetSection(SmtpServer.SectionName));
        return services;
    }
}
