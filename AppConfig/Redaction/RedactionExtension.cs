namespace FinBookeAPI.AppConfig.Redaction;

public static class RedactionExtension
{
    public static IServiceCollection AddRedactionExt(this IServiceCollection collection)
    {
        collection.AddRedaction(redactionBuilder =>
        {
            redactionBuilder.SetRedactor<StarRedactor>(
                RedactionClassification.Private,
                RedactionClassification.Personal
            );
        });
        return collection;
    }
}
