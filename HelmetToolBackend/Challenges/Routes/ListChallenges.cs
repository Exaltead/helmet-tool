using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;


namespace HelmetToolBackend.Challenges.Routes;

public class ListChallenges(ILogger<ListChallenges> _logger, IJwtHandler _jwtHandler, IChallengeStorage _challengeStorage)
{

    [Function("ListChallenges")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "challenge")] HttpRequest req)
    {
        var user = _jwtHandler.GetUser(req);

        if (user == null)
        {
            _logger.LogWarning("User not authenticated.");
            return new UnauthorizedResult();
        }

        _logger.LogInformation("User {user} tried list challenges", user?.Username);


        var challenges = await _challengeStorage.GetChallenges();

        return new OkObjectResult(new { challenges });
    }
}
