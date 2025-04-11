using HelmetToolBackend.Auth;
using HelmetToolBackend.Challenges;
using HelmetToolBackend.Library;
using HelmetToolBackend.Storage;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Register your services here
        services.AddSingleton<IAuthClient, AuthClient>();
        services.AddSingleton<IJwtHandler, JwtHandler>();
        services.AddSingleton<ILibraryStorage, LibraryStorage>();
        services.AddSingleton<IChallengeStorage, ChallengeStorage>();
        services.AddSingleton((_) =>
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString")
                ?? throw new InvalidOperationException("CosmosDB connection string is not set.");
            return new CosmosClient(connectionString, new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                }
            });
        });
    })
    .Build();

host.Run();
