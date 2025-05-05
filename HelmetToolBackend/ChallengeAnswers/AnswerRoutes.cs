using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;


namespace HelmetToolBackend.ChallengeAnswers;

public class ChallengeRoutes(ILogger<ChallengeRoutes> _logger, IJwtHandler _jwtHandler, IAnswerStorage _answerStorage) : BaseRoute<ChallengeAnswerSet>(_logger, _jwtHandler)
{
    private static List<AnswerRecord> UpdateAnswersState(IEnumerable<AnswerRecord> answers)
    {
        return [.. answers.Select(a =>
        {
            if (string.IsNullOrEmpty(a.Id.Trim()))
            {
                a.Id = Guid.NewGuid().ToString();
            }

            if (a.Kind == "Boolean")
            {
                a.Answered = true;
            }
            else if (a.Kind == "TextInput")
            {
                a.Answer = a.Answer.Trim();
                a.Answered = !string.IsNullOrEmpty(a.Answer);
            }
            return a;
        })];
    }

    [Function("AddAnswer")]
    public async Task<IActionResult> AddAnswer([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "answer")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, answer) =>
        {
            var newItem = answer with { Id = Guid.NewGuid().ToString(), UserId = user.Id };

            newItem.Answers = UpdateAnswersState(newItem.Answers);

            var id = await _answerStorage.AddAnswerSet(newItem);

            return new OkObjectResult(new { id });
        });

    }
    [Function("GetAnswers")]
    public async Task<IActionResult> ListChallenges([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "answer")] HttpRequest req)
    {
        var challengeId = req.Query["challengeId"].ToString();
        var itemId = req.Query["itemId"].ToString();
        if (string.IsNullOrEmpty(challengeId))
        {
            _logger.LogWarning("ChallengeId or ItemId is null or empty.");
            return new BadRequestObjectResult("ChallengeId or ItemId is null or empty.");
        }

        return await WithUser(req, async user =>
        {
            _logger.LogInformation("User {user} tried list challenges", user.Username);
            if (!string.IsNullOrEmpty(itemId))
            {
                var answers = await _answerStorage.GetAnswers(user.Id, challengeId, itemId);

                return new OkObjectResult(new { answers });
            }
            else
            {
                var answers = await _answerStorage.GetAnswers(user.Id, challengeId);

                var onlyAnswers = answers.SelectMany(x =>
                {
                    var mapped = x.Answers.Select(a => a with { ItemId = x.ItemId });
                    return mapped;
                }).ToList();

                return new OkObjectResult(onlyAnswers);
            }


        });
    }

    [Function("UpdateAnswer")]
    public async Task<IActionResult> UpdateChallenge([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "answer")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, answer) =>
        {
            if (string.IsNullOrEmpty(answer.Id))
            {
                _logger.LogWarning("Answer id is null or empty.");
                return new BadRequestObjectResult("Answer id is null or empty.");
            }

            var existingAnswer = await _answerStorage.GetAnswer(answer.Id, user.Id);
            if (existingAnswer == null)
            {
                return new NotFoundObjectResult("Answer not found.");
            }

            if (existingAnswer.UserId != user.Id)
            {
                Console.WriteLine($"{user.Id} != {answer.UserId}");
                _logger.LogWarning("User {user} tried to update answer {answerId} that does not belong to them.", user.Username, answer.Id);
                return new StatusCodeResult(403);
            }

            await _answerStorage.UpdateAnswers(answer with { UserId = user.Id, Answers = UpdateAnswersState(answer.Answers) });

            return new OkObjectResult("Update successful");
        });
    }
}
