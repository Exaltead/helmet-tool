using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;


namespace HelmetToolBackend.Challenges;

public class ChallengeRoutes(ILogger<ChallengeRoutes> _logger, IJwtHandler _jwtHandler, IChallengeStorage _challengeStorage) : BaseRoute<Challenge>(_logger, _jwtHandler)
{
    [Function("AddChallenge")]
    public async Task<IActionResult> AddChallenge([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "challenge")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, challenge) =>
        {
            var newItem = challenge with { Id = Guid.NewGuid().ToString() };

            var id = await _challengeStorage.AddChallenge(newItem);

            return new OkObjectResult(new { id });
        });

    }
    [Function("ListChallenge")]
    public async Task<IActionResult> ListChallenges([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "challenge")] HttpRequest req)
    {
        return await WithUser(req, async user =>
        {
            _logger.LogInformation("User {user} tried list challenges", user?.Username);
            var challenges = await _challengeStorage.GetChallenges();

            return new OkObjectResult(new { challenges });
        });
    }

    [Function("UpdateChallenge")]
    public async Task<IActionResult> UpdateChallenge([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "challenge")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, challenge) =>
        {
            if (string.IsNullOrEmpty(challenge.Id))
            {
                _logger.LogWarning("Challenge id is null or empty.");
                return new BadRequestObjectResult("Challenge id is null or empty.");
            }

            await _challengeStorage.UpdateChallenge(challenge);

            return new OkObjectResult("Update successful");
        });
    }
}
