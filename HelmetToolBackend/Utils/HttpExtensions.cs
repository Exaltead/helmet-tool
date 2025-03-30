using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace HelmetToolBackend.Utils
{
    public static class HttpExtensions
    {
        public static async Task<T?> GetBody<T>(this HttpRequest req)
        {
            try
            {
                using var reader = new StreamReader(req.Body);
                var body = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                };

                return JsonSerializer.Deserialize<T>(body, options);
            }
            catch
            {
                return default;
            }
        }

    }
}