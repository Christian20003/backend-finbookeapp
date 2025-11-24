using NReco.Logging.File;

namespace FinBookeAPI.AppConfig.Documentation;

public static class LoggingExtension
{
    /// <summary>
    /// This method adds a logger to the application with some configurations. This method supports the following
    /// logging types:
    /// <list type="bullet">
    ///     <item><c>Console</c></item>
    ///     <item>
    ///         <c>File</c>: It will print each log message in a seperate JSON object (see configurations in
    ///         <c>appsettings.json</c>)
    ///         <code>
    ///             {
    ///                 "log": {
    ///                     "time": "[TIME]",
    ///                     "level": "[LOG_LEVEL]",
    ///                     "name": "[LOG_NAME]",
    ///                     "event": "[EVENT_ID]",
    ///                     "message": "[MESSAGE]",
    ///                     "exception": "[EXCEPTION || NULL]"
    ///                 }
    ///             }
    ///         </code>
    ///     </item>
    /// </list>
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">A set of key/value application configuration properties.</param>
    /// <returns>The modified <c>IServiceCollection</c></returns>
    public static IServiceCollection AddLogger(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddLogging(loggingBuilder =>
        {
            // Get config from appsettings.json
            var loggingSettings = configuration.GetSection("Logging");
            // Remove all current logging settings
            loggingBuilder.ClearProviders();
            loggingBuilder.EnableRedaction();
            // Add file logging
            loggingBuilder.AddFile(
                loggingSettings,
                fileLoggerOptions =>
                {
                    // Configure structure
                    fileLoggerOptions.FormatLogEntry = (msg) =>
                    {
                        var builder = new System.Text.StringBuilder();
                        var writer = new StringWriter(builder);
                        var json = new Newtonsoft.Json.JsonTextWriter(writer);
                        json.WriteStartObject();
                        json.WritePropertyName("log");
                        json.WriteStartObject();
                        json.WritePropertyName("time");
                        json.WriteValue(DateTime.Now.ToString("O"));
                        json.WritePropertyName("level");
                        json.WriteValue(msg.LogLevel.ToString());
                        json.WritePropertyName("name");
                        json.WriteValue(msg.LogName);
                        json.WritePropertyName("event");
                        json.WriteValue(msg.EventId.Id);
                        json.WritePropertyName("message");
                        json.WriteValue(msg.Message);
                        json.WritePropertyName("exception");
                        json.WriteValue(msg.Exception?.ToString());
                        json.WriteEndObject();
                        json.WriteEndObject();
                        return builder.ToString();
                    };
                }
            );
            // Add console logging
            loggingBuilder.AddConsole();
        });
        return services;
    }
}
