using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HelmetToolBackend.ChallengeAnswers;

public class AnswerStorage(ILoggerFactory loggerFactory, CosmosClient client, Config config) :
    CrudStorage<ChallengeAnswerSet>(loggerFactory, client,
    config.CosmosDbDatabaseName, config.CosmosDbContainerNameAnswers,
    (t) => t.UserId, "answer"), IAnswerStorage
{

    public Task<string> AddAnswerSet(ChallengeAnswerSet answers)
    {
        return AddEntity(answers);
    }

    public Task<List<ChallengeAnswerSet>> GetAnswers(string userId, string challengeId, string itemId)
    {
        var query = new QueryDefinition("SELECT * FROM answers a WHERE a.userId = @userId AND a.challengeId = @challengeId AND a.itemId = @itemId")
            .WithParameter("@userId", userId)
            .WithParameter("@challengeId", challengeId)
            .WithParameter("@itemId", itemId);

        return ListEntities(query);
    }

    public async Task UpdateAnswers(ChallengeAnswerSet answer)
    {
        await UpdateEntity(answer);
    }

    public async Task DeleteAnswer(string id, string userId)
    {
        await DeleteEntity(id, userId);
    }

    public async Task<ChallengeAnswerSet?> GetAnswer(string id, string userId)
    {
        return await GetEntity(id, userId);
    }

    public Task<List<ChallengeAnswerSet>> GetAnswers(string userId, string challengeId)
    {
        var query = new QueryDefinition("SELECT * FROM answers a WHERE a.userId = @userId AND a.challengeId = @challengeId")
            .WithParameter("@userId", userId)
            .WithParameter("@challengeId", challengeId);

        return ListEntities(query);
    }
}
