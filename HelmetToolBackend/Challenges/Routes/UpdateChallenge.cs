using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;
using HelmetToolBackend.Models;


namespace HelmetToolBackend.Challenges.Routes
{
    public class UpdateChallenge(ILogger<UpdateChallenge> _logger, IJwtHandler _jwtHandler, IChallengeStorage _challengeStorage)
    {

        [Function("UpdateChallenge")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "challenge")] HttpRequest req)
        {
            var user = _jwtHandler.GetUser(req);

            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            _logger.LogInformation("User {user} tried to update a challenge.", user?.Username);

            var challenge = await req.GetBody<Challenge>();

            if (challenge == null)
            {
                _logger.LogWarning("Challenge is null.");
                return new BadRequestObjectResult("Challenge is null.");
            }

            if (string.IsNullOrEmpty(challenge.Id))
            {
                _logger.LogWarning("Challenge id is null or empty.");
                return new BadRequestObjectResult("Challenge id is null or empty.");
            }

            var id = await _challengeStorage.UpdateChallenge(challenge);

            return new OkObjectResult(new { id });
        }
    }
}
