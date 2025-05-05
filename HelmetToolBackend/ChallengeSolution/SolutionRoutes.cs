using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Shared;
using HelmetToolBackend.Models;

namespace HelmetToolBackend.ChallengeSolution;


public class ChallengeSolutionRoutes(ILogger<ChallengeSolutionRoutes> _logger, IJwtHandler _jwtHandler, ISolutionStorage _solutionStorage) :
    BaseRoute<SolutionSet>(_logger, _jwtHandler)
{
    [Function("AddChallengeSolution")]
    public async Task<IActionResult> AddChallengeSolution([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "solution")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, solution) =>
        {
            var newItem = solution with { Id = Guid.NewGuid().ToString(), UserId = user.Id };

            var id = await _solutionStorage.AddSolutionSet(newItem);

            return new OkObjectResult(new { id });
        });
    }

    [Function("GetChallengeSolutions")]
    public async Task<IActionResult> GetChallengeSolutions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "solution")] HttpRequest req)
    {

        return await WithUser(req, async user =>
        {
            if (req.Query.TryGetValue("solutionId", out var solution) && !string.IsNullOrEmpty(solution))
            {
                var solutions = await _solutionStorage.GetSolutionSet(solution!, user.Id);
                if (solutions == null)
                {
                    return new NotFoundObjectResult("Solution not found.");
                }
                if (solutions.UserId != user.Id)
                {
                    _logger.LogWarning("User {user} tried to get solution {solutionId} that does not belong to them.", user.Username, solution);
                    return new StatusCodeResult(403);
                }
                return new OkObjectResult(new[] { solutions });
            }
            if (req.Query.TryGetValue("challengeId", out var challengeId) && !string.IsNullOrEmpty(challengeId))
            {
                var solutions = await _solutionStorage.GetSolutionSetByChallengeId(challengeId!, user.Id);
                if (solutions == null)
                {
                    return new OkObjectResult(Array.Empty<SolutionSet>());
                }
                if (solutions.UserId != user.Id)
                {
                    _logger.LogWarning("User {user} tried to get solution {solutionId} that does not belong to them.", user.Username, challengeId);
                    return new StatusCodeResult(403);
                }
                return new OkObjectResult(new[] { solutions });
            }
            else
            {
                _logger.LogInformation("User {user} tried to list challenge solutions", user.Username);
                var solutions = await _solutionStorage.GetSolutionSets(user.Id);

                return new OkObjectResult(solutions ?? []);
            }

        });
    }

    [Function("UpdateChallengeSolution")]
    public async Task<IActionResult> UpdateChallengeSolution([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "solution/{id}")] HttpRequest req, string id)
    {
        return await WithUserAndBody(req, async (user, solution) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Solution id is null or empty.");
                return new BadRequestObjectResult("Solution id is null or empty.");
            }

            solution.Id = id;

            var existingSolution = await _solutionStorage.GetSolutionSet(solution.Id, user.Id);
            if (existingSolution == null)
            {
                return new NotFoundObjectResult("Solution not found.");
            }

            if (existingSolution.UserId != user.Id)
            {
                _logger.LogWarning("User {user} tried to update solution {solutionId} that does not belong to them.", user.Username, solution.Id);
                return new StatusCodeResult(403);
            }

            await _solutionStorage.UpdateSolutionSet(solution with { UserId = user.Id });

            return new OkObjectResult("Update successful");
        });
    }
}
