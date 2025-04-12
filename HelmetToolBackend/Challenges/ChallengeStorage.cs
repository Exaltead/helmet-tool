using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HelmetToolBackend.Challenges;

public class ChallengeStorage(ILoggerFactory loggerFactory, CosmosClient client, Config config) :
    CrudStorage<Challenge>(loggerFactory, client,
    config.CosmosDbDatabaseName, config.CosmosDbContainerNameChallenge,
    (t) => t.Id, "challenge"), IChallengeStorage
{


    public async Task<string> AddChallenge(Challenge challenge)
    {
        return await AddEntity(challenge);
    }

    public async Task<List<Challenge>> GetChallenges()
    {
        var query = new QueryDefinition("SELECT * FROM challenges c");
        return await ListEntities(query);
    }

    public async Task UpdateChallenge(Challenge challenge)
    {
        await UpdateEntity(challenge);
    }

    public async Task DeleteChallenge(string id)
    {
        await DeleteEntity(id, id);
    }
}
