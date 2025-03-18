using System.Net;
using FinBookeAPI.DTO.Error;
using Newtonsoft.Json;

namespace FinBookeAPI.Middleware;

public class BadRequestHandling : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.ContentType = "application/json";
        var original = context.Response.Body;
        using var stream = new MemoryStream();
        context.Response.Body = stream;

        await next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream, leaveOpen: true))
            {
                var content = await reader.ReadToEndAsync();
                var body = JsonConvert.DeserializeObject<BadRequestResponse>(content);
                var newBody = new ErrorResponse
                {
                    Type = "StructureException",
                    Title = "Faulty message structure",
                    Status = context.Response.StatusCode,
                    Instance = context.Request.Path,
                };
                var message = "The follwing properties are missing or incorrect: ";
                var props = new List<string>();
                foreach (var entry in body.Errors)
                {
                    props.Add(entry.Key);
                    message += entry.Key + ", ";
                }
                newBody.Detail = message.TrimEnd(',', ' ');
                newBody.InvalidProps = props;
                //await context.Response.WriteAsync(JsonConvert.SerializeObject(newBody));
                using var newStream = new MemoryStream();
                using var writer = new StreamWriter(newStream, leaveOpen: true);
                await writer.WriteAsync(JsonConvert.SerializeObject(newBody));
                await writer.FlushAsync();
                context.Response.ContentLength = newStream.Length;
                newStream.Seek(0, SeekOrigin.Begin);
                await newStream.CopyToAsync(original);
                //context.Response.Body = original;
            }
            ;
        }
    }
}
