namespace HelmetToolBackend.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HelmetToolBackend.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;

    public interface IDbEntity
    {
        string Id { get; set; }
    }


    public abstract class CrudStorage<T>(ILoggerFactory loggerFactory,
        CosmosClient client, string dbName, string containerName, Func<T, string> getPartitionKey, string entityName)
        where T : class?, IDbEntity
    {
        protected readonly ILogger Logger = loggerFactory.CreateLogger<T>();

        protected readonly Container Container = client.GetContainer(dbName, containerName);

        public async Task<string> AddEntity(T entity)
        {
            var response = await Container.CreateItemAsync(entity, new PartitionKey(getPartitionKey(entity)));
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Logger.LogError("Failed to add {entityName} with id {id}.", entityName, entity.Id);
                throw new Exception($"Failed to add {entityName} with id {entity.Id}.");
            }
            Logger.LogInformation("Added {entityName} with id {id}.", entityName, entity.Id);

            return entity.Id;
        }


        public async Task<T?> GetEntity(string id, string partitionKey)
        {
            try
            {
                var response = await Container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Resource;
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Logger.LogWarning("{entityName} with id {id} not found.", entityName, id);
            }

            return null;
        }

        public async Task DeleteEntity(string id, string partitionId)
        {
            var response = await Container.DeleteItemAsync<LibraryItem>(id, new PartitionKey(partitionId));
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                Logger.LogError("Failed to delete {entityName} with id {id}.", entityName, id);
                throw new Exception($"Failed to delete {entityName} with id {id}.");
            }
            Logger.LogInformation("Deleted {entityName} with id {id}.", entityName, id);
        }

        public async Task UpdateEntity(T item)
        {
            var response = await Container.ReplaceItemAsync(item, item.Id, new PartitionKey(getPartitionKey(item)));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.LogError("Failed to update {entityName} with id {id}.", entityName, item.Id);
                throw new Exception($"Failed to update {entityName} with id {item.Id}.");
            }
            Logger.LogInformation("Updated {entityName} with id {id}.", entityName, item.Id);
        }

        public async Task<List<T>> ListEntities(QueryDefinition query)
        {
            try
            {
                var iterator = Container.GetItemQueryIterator<T>(query);
                var results = new List<T>();

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }

                return results;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Logger.LogWarning("{entityName} not found.", entityName);
                return [];
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to list {entityName}: {message}", entityName, ex.Message);
                throw;
            }

        }
    }
}

