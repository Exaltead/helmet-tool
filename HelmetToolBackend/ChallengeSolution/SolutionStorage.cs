using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HelmetToolBackend.ChallengeSolution;

public class SolutionStorage(ILoggerFactory loggerFactory, CosmosClient client, Config config) :
    CrudStorage<SolutionSet>(loggerFactory, client,
    config.CosmosDbDatabaseName, config.CosmosDbContainerNameSolutions,
    (t) => t.UserId, "´solution"), ISolutionStorage
{
    public Task<string> AddSolutionSet(SolutionSet solution)
    {
        return AddEntity(solution);
    }

    public Task<List<SolutionSet>> GetSolutionSets(string userId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId AND c.challengeId = @challengeId AND c.itemId = @itemId")
            .WithParameter("@userId", userId);

        return ListEntities(query);
    }

    public async Task UpdateSolutionSet(SolutionSet solution)
    {
        await UpdateEntity(solution);
    }

    public async Task DeleteSolutionSet(string id, string userId)
    {
        await DeleteEntity(id, userId);
    }

    public async Task<SolutionSet?> GetSolutionSet(string id, string userId)
    {
        return await GetEntity(id, userId);
    }

    public async Task<SolutionSet?> GetSolutionSetByChallengeId(string challengeId, string userId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId AND c.challengeId = @challengeId")
            .WithParameter("@userId", userId)
            .WithParameter("@challengeId", challengeId);

        var found = await ListEntities(query);

        if (found.Count == 0)
        {
            return null;
        }
        if (found.Count > 1)
        {
            throw new Exception($"Found more than one solution for challenge {challengeId} for user {userId}");
        }
        return found[0];

    }
}
