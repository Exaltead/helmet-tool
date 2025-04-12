
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;

namespace HelmetToolBackend.Auth;



internal record UserWithPassword(string Id, string Username, string PasswordHash);


public class AuthClient(ILoggerFactory loggerFactory, CosmosClient client, Config config) : IAuthClient
{
    private readonly Container _container = client.GetContainer(config.CosmosDbDatabaseName, config.CosmosDbContainerNameUsers);
    private readonly ILogger _logger = loggerFactory.CreateLogger<AuthClient>();

    public async Task<Models.User?> ValidLogin(string username, string password)
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
        return new Models.User(user.Id, user.Username);
    }

    private string getSha256Hash(string password)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}