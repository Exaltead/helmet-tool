using System.Text.Json;
using System.Text.Json.Serialization;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Storage;
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

        public static string GetBearerToken(this HttpRequest req)
        {
            var authHeader = req.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return string.Empty;
            }

            return authHeader["Bearer ".Length..].Trim();
        }

        public static User? GetUser(this IJwtHandler jwtHandler, HttpRequest req)
        {
            var token = req.GetBearerToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var user = jwtHandler.VerifyAndDecode(token);
            if (user == null)
            {
                return null;
            }

            return user;

        }

    }
}