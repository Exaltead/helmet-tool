
using HelmetToolBackend.Storage;
using HelmetToolBackend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

using Microsoft.Extensions.Logging;

namespace HelmetToolBackend.Auth
{
    internal class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    internal class AuthResponse(string token)
    {
        public string Token { get; } = token;
    }

    public class Login
    {
        private readonly ILogger _logger;
        private readonly IAuthClient _authClient;

        public Login(ILoggerFactory loggerFactory, IAuthClient authClient)
        {
            _logger = loggerFactory.CreateLogger<Login>();
            _authClient = authClient;
        }

        [Function("login")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("User tried to login.");

            var loginRequest = await req.GetBody<LoginRequest>();
            if (loginRequest == null)
            {
                return new BadRequestObjectResult("Invalid request body.");
            }

            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return new BadRequestObjectResult("Username and password are required.");
            }

            var validLogin = await _authClient.ValidLogin(loginRequest.Username, loginRequest.Password);

            if (!validLogin)
            {
                return new UnauthorizedResult();
            }

            var auth = new AuthResponse("KekkosToken");

            return new OkObjectResult(auth);
        }
    }
}
