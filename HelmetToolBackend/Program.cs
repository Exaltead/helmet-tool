using HelmetToolBackend.Auth;
using HelmetToolBackend.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Register your services here
        services.AddSingleton<IAuthClient, AuthClient>();
        services.AddSingleton<IJwtHandler, JwtHandler>();
    })
    .Build();

host.Run();
