using HelmetToolBackend.Auth;
using HelmetToolBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HelmetToolBackend.Utils;

namespace HelmetToolBackend.Shared;

public abstract class BaseRoute<T>(ILogger logger, IJwtHandler jwtHandler)
{
    protected readonly ILogger Logger = logger;
    protected readonly IJwtHandler JwtHandler = jwtHandler;

    protected async Task<IActionResult> WithUser(HttpRequest req, Func<User, Task<IActionResult>> action)
    {
        var user = JwtHandler.GetUser(req);

        if (user == null)
        {
            Logger.LogWarning("User not authenticated.");
            return new UnauthorizedResult();
        }


        return await action(user);
    }

    protected async Task<IActionResult> WithBody(HttpRequest req, Func<T, Task<IActionResult>> action)
    {
        var body = await req.GetBody<T>();

        if (body == null)
        {
            Logger.LogWarning("{name} is null.", typeof(T).Name);
            return new BadRequestObjectResult($"{typeof(T).Name} is null.");
        }

        return await action(body);
    }

    protected async Task<IActionResult> WithUserAndBody(HttpRequest req, Func<User, T, Task<IActionResult>> action)
    {
        var user = JwtHandler.GetUser(req);

        if (user == null)
        {
            Logger.LogWarning("User not authenticated.");
            return new UnauthorizedResult();
        }

        var body = await req.GetBody<T>();

        if (body == null)
        {
            Logger.LogWarning("{name} is null.", typeof(T).Name);
            return new BadRequestObjectResult($"{typeof(T).Name} is null.");
        }

        return await action(user, body);
    }


}