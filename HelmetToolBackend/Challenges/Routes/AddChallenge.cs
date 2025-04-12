using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;
using HelmetToolBackend.Models;


namespace HelmetToolBackend.Challenges.Routes;

public class AddChallenge(ILogger<AddChallenge> _logger, IJwtHandler _jwtHandler, IChallengeStorage _challengeStorage)
{

    [Function("AddChallenge")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "challenge")] HttpRequest req)
    {
        var user = _jwtHandler.GetUser(req);

        if (user == null)
        {
            _logger.LogWarning("User not authenticated.");
            return new UnauthorizedResult();
        }

        _logger.LogInformation("User {user} tried to add a challenge.", user?.Username);

        var challenge = await req.GetBody<Challenge>();

        if (challenge == null)
        {
            _logger.LogWarning("Challenge is null.");
            return new BadRequestObjectResult("Challenge is null.");
        }

        var newItem = challenge with { Id = Guid.NewGuid().ToString() };

        var id = await _challengeStorage.AddChallenge(newItem);

        return new OkObjectResult(new { id });
    }
}
