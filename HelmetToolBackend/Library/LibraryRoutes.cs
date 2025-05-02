using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Models;
using HelmetToolBackend.Shared;

namespace HelmetToolBackend.Library;

public class LibraryRoutes(ILogger<LibraryRoutes> _logger, IJwtHandler _jwtHandler, ILibraryStorage _libraryStorage) : BaseRoute<LibraryItem>(_logger, _jwtHandler)
{
    [Function("AddLibraryItem")]
    public async Task<IActionResult> AddLibraryItem([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "library")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, libraryItem) =>
        {
            var newItem = libraryItem with { UserId = user.Id, Id = Guid.NewGuid().ToString() };

            var id = await _libraryStorage.AddLibraryItem(newItem);

            return new OkObjectResult(new { id });
        });
    }

    [Function("ListLibraryItems")]
    public async Task<IActionResult> ListLibraryItems([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "library")] HttpRequest req)
    {
        return await WithUser(req, async user =>
        {
            _logger.LogInformation("User {user} tried list library items", user.Username);
            var libraryItems = await _libraryStorage.GetAllLibraryItems(user.Id);

            return new OkObjectResult(libraryItems);
        });
    }

    [Function("UpdateLibraryItem")]
    public async Task<IActionResult> UpdateLibraryItem([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "library")] HttpRequest req)
    {
        return await WithUserAndBody(req, async (user, libraryItem) =>
        {
            if (string.IsNullOrEmpty(libraryItem.Id))
            {
                _logger.LogWarning("Library item id is null or empty.");
                return new BadRequestObjectResult("Library item id is null or empty.");
            }

            if (!await IsUsersLibraryItem(libraryItem.Id, user))
            {
                return new ForbidResult();
            }

            libraryItem.UserId = user.Id;

            await _libraryStorage.UpdateLibraryItem(libraryItem);

            return new OkObjectResult("Update successful");
        });
    }

    [Function("DeleteLibraryItem")]
    public async Task<IActionResult> DeleteLibraryItem([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "library/{itemId}")] HttpRequest req, string itemId)
    {
        return await WithUser(req, async (user) =>
        {
            _logger.LogInformation("User {user} tried to delete a library item {itemId}.", user.Username, itemId);

            if (!await IsUsersLibraryItem(itemId, user))
            {
                return new ForbidResult();
            }

            await _libraryStorage.DeleteLibraryItem(itemId, user.Id);
            _logger.LogInformation("Library item {itemId} deleted.", itemId);

            return new OkObjectResult("Delete successful");
        });
    }

    private async Task<bool> IsUsersLibraryItem(string itemId, User user)
    {
        var item = await _libraryStorage.GetLibraryItem(itemId, user.Id);
        if (item == null)
        {
            _logger.LogWarning("Library item with id {id} not found.", itemId);
            return false;
        }

        if (item.UserId != user.Id)
        {
            _logger.LogWarning("User {user} tried to modify/delete a library item {itemId} that does not belong to them.", user.Username, itemId);
            return false;
        }

        return true;
    }

}
