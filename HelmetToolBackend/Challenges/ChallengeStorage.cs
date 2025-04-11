using HelmetToolBackend.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HelmetToolBackend.Challenges
{
    public class ChallengeStorage(ILogger<ChallengeStorage> logger, CosmosClient client) : IChallengeStorage
    {
        private readonly ILogger<ChallengeStorage> _logger = logger;
        private readonly Container _container = client.GetContainer("helmet-storage", "challenges");

        public async Task<string> AddChallenge(Challenge challenge)
        {
            var response = await _container.CreateItemAsync(challenge, new PartitionKey(challenge.Id));
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                _logger.LogError("Failed to add challenge with id {id}.", challenge.Id);
                throw new Exception($"Failed to add challenge with id {challenge.Id}.");
            }
            _logger.LogInformation("Added challenge with id {id}.", challenge.Id);

            return challenge.Id;
        }

        public async Task<List<Challenge>> GetChallenges()
        {
            var query = new QueryDefinition("SELECT * FROM challenges c");
            var iterator = _container.GetItemQueryIterator<Challenge>(query);
            var results = new List<Challenge>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<bool> UpdateChallenge(Challenge challenge)
        {
            var response = await _container.ReplaceItemAsync(challenge, challenge.Id, new PartitionKey(challenge.Id));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError("Failed to update challenge with id {id}.", challenge.Id);
                return false;
            }
            _logger.LogInformation("Updated challenge with id {id}.", challenge.Id);
            return true;
        }

        public async Task<bool> DeleteChallenge(string id)
        {
            var response = await _container.DeleteItemAsync<Challenge>(id, new PartitionKey(id));
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogError("Failed to delete challenge with id {id}.", id);
                return false;
            }
            _logger.LogInformation("Deleted challenge with id {id}.", id);
            return true;
        }
    }
}
