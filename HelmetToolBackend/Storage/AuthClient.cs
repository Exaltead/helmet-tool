using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace HelmetToolBackend.Storage
{


    internal record UserWithPassword(string Id, string Username, string PasswordHash);


    public class AuthClient : IAuthClient
    {
        private readonly CosmosClient _client;
        private readonly Container _container;
        private readonly ILogger _logger;

        public AuthClient(ILoggerFactory loggerFactory)
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString") ?? throw new InvalidOperationException("CosmosDB connection string is not set.");

            _client = new CosmosClient(connectionString);
            _container = _client.GetContainer("helmet-storage", "users");
            _logger = loggerFactory.CreateLogger<AuthClient>();
        }

        public async Task<User?> ValidLogin(string username, string password)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.username = @username")
                .WithParameter("@username", username);

            var iterator = _container.GetItemQueryIterator<UserWithPassword>(query);
            var results = await iterator.ReadNextAsync();

            if (results.Count == 0)
            {
                _logger.LogWarning("User {Username} not found.", username);
                return null;
            }

            var user = results.First();
            var passwordHash = getSha256Hash(password);

            if (user.PasswordHash != passwordHash)
            {
                _logger.LogWarning("Invalid password for user {Username}.", username);
                return null;
            }

            _logger.LogInformation("User {Username} logged in successfully.", username);
            return new User(user.Id, user.Username);
        }

        private string getSha256Hash(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}