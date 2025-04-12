namespace HelmetToolBackend.Library
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HelmetToolBackend.Models;
    using HelmetToolBackend.Shared;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;



    public class LibraryStorage(ILoggerFactory loggerFactory, CosmosClient client, Config config)
    : CrudStorage<LibraryItem>(loggerFactory, client, config.CosmosDbDatabaseName, "library", item => item.UserId!, "library"),
    ILibraryStorage
    {
        public async Task<string> AddLibraryItem(LibraryItem item)
        {
            return await AddEntity(item);
        }
        public async Task<List<LibraryItem>> GetAllLibraryItems(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM library l WHERE l.userId = @userId")
                .WithParameter("@userId", userId);

            return await ListEntities(query);
        }

        public async Task<LibraryItem?> GetLibraryItem(string id, string userId)
        {
            return await GetEntity(id, userId);
        }

        public async Task DeleteLibraryItem(string id, string userId)
        {
            await DeleteEntity(id, userId);
        }

        public async Task UpdateLibraryItem(LibraryItem item)
        {
            await UpdateEntity(item);
        }
    }
}

