using System.Text;
using HelmetToolBackend.Storage;

namespace HelmetToolBackend.Auth
{


    public class JwtHandler : IJwtHandler
    {

        private readonly string _secretKey = "Salaisuuuuuus";
        public string SignUserToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "username", user.Username },
                { "exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds() }
            };


            var secretKey = Encoding.ASCII.GetBytes(_secretKey);



            var token = Jose.JWT.Encode(payload, secretKey, Jose.JwsAlgorithm.HS256);

            return token;
        }

        public User? VerifyAndDecode(string token)
        {
            var secretKey = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                var payload = Jose.JWT.Decode<Dictionary<string, object>>(token, secretKey);

                if (!payload.ContainsKey("exp") || (long)payload["exp"] < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    return null; // Token has expired
                }

                return new User((string)payload["id"], (string)payload["username"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}