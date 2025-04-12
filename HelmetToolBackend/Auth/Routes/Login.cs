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

    public class Login(ILoggerFactory loggerFactory, IAuthClient authClient, IJwtHandler jwtHandler)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<Login>();
        private readonly IAuthClient _authClient = authClient;
        private readonly IJwtHandler _jwtHandler = jwtHandler;

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

            if (validLogin == null)
            {
                _logger.LogWarning("Invalid login attempt for user {Username}.", loginRequest.Username);
                return new UnauthorizedResult();
            }

            var token = _jwtHandler.SignUserToken(validLogin);

            var auth = new AuthResponse(token);

            return new OkObjectResult(auth);
        }

    }
}
