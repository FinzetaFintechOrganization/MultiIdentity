using System.Diagnostics;
using System.Text.Json;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Stopwatch ile süreyi başlat
        var stopwatch = Stopwatch.StartNew();

        // Response'u yakalamak için bir geçici bellek oluştur
        var originalResponseBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            // Diğer middleware'leri çağır
            await _next(context);
        }
        finally
        {
            // Süreyi durdur
            stopwatch.Stop();
        }

        // Yanıt gövdesini oku ve API süresini ekle
        memoryStream.Seek(0, SeekOrigin.Begin);
        var originalResponseBody = await new StreamReader(memoryStream).ReadToEndAsync();
        memoryStream.Seek(0, SeekOrigin.Begin);

        // Eğer yanıt JSON ise süreyi ekle
        if (context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
        {
            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(originalResponseBody);

            if (jsonResponse != null)
            {
                jsonResponse["elapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;

                var updatedResponse = JsonSerializer.Serialize(jsonResponse);

                context.Response.Body = originalResponseBodyStream;
                context.Response.ContentLength = updatedResponse.Length;
                await context.Response.WriteAsync(updatedResponse);

                return;
            }
        }

        // Eğer yanıt JSON değilse orijinal yanıtı geri yaz
        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalResponseBodyStream);
    }
}
