using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;


namespace HelmetToolBackend.Challenges
{
    public record Challenge
    {
        public string Question { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }

    public class GetChallenges(ILoggerFactory loggerFactory, IJwtHandler jwtHandler)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<HttpExample>();
        private readonly IJwtHandler _jwtHandler = jwtHandler;

        [Function("GetChallenges")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            var user = _jwtHandler.GetUser(req);
            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            _logger.LogInformation("Getting challenge questions by {user}.", user?.Username);

            var demoChallenges = new List<Challenge>
            {
                new() { Question = "Oliko hyvä kirja?", Id = "1" },
                new() { Question = "Suosittelisitko?", Id = "2" },
                new() { Question = "Sisälsikö kettuja?", Id = "3" }
            };

            return new OkObjectResult(demoChallenges);
        }
    }
}
