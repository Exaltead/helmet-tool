namespace HelmetToolBackend.Library
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HelmetToolBackend.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;



    public class LibraryStorage : ILibraryStorage
    {
        private readonly ILogger _logger;

        private readonly Container _container;

        public LibraryStorage(ILoggerFactory loggerFactory, CosmosClient client)
        {
            _container = client.GetContainer("helmet-storage", "library");
            _logger = loggerFactory.CreateLogger<LibraryStorage>();
        }

        public async Task AddLibraryItem(LibraryItem item)
        {
            var response = await _container.CreateItemAsync(item, new PartitionKey(item.UserId));
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                _logger.LogError("Failed to add library item with id {id}.", item.Id);
                throw new Exception($"Failed to add library item with id {item.Id}.");
            }
            _logger.LogInformation("Added library item with id {id}.", item.Id);
        }
        public async Task<List<LibraryItem>> GetAllLibraryItems(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM library l WHERE l.userId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<LibraryItem>(query);
            var results = new List<LibraryItem>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<LibraryItem?> GetLibraryItem(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<LibraryItem>(id, new PartitionKey(id));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Resource;
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Library item with id {id} not found.", id);
            }

            return null;
        }

        public async Task DeleteLibraryItem(string id)
        {
            var response = await _container.DeleteItemAsync<LibraryItem>(id, new PartitionKey(id));
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogError("Failed to delete library item with id {id}.", id);
                throw new Exception($"Failed to delete library item with id {id}.");
            }
            _logger.LogInformation("Deleted library item with id {id}.", id);
        }
    }
}

