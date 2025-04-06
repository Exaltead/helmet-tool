using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;
using HelmetToolBackend.Library;
using HelmetToolBackend.Models;

namespace HelmetToolBackend.Library.Routes
{
    public class UpdateLibraryItem(ILogger<AddLibraryItem> _logger, IJwtHandler _jwtHandler, ILibraryStorage _libraryStorage)
    {
        [Function("UpdateLibraryItem")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "library/{itemId}")] HttpRequest req, string itemId)
        {
            var user = _jwtHandler.GetUser(req);

            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            var libraryItem = await req.GetBody<LibraryItem>();

            if (libraryItem == null)
            {
                _logger.LogWarning("Library item is null.");
                return new BadRequestObjectResult("Library item is null.");
            }

            _logger.LogInformation("User {user} tried to update a library item.", user?.Username);
            var existing = await _libraryStorage.GetLibraryItem(itemId, user!.Id);
            if (existing == null)
            {
                _logger.LogWarning("Library item with id {id} not found.", itemId);
                return new NotFoundResult();
            }
            if (existing.UserId != user!.Id)
            {
                _logger.LogWarning("User {user} tried to add a library item {itemId} that does not belong to them.", user?.Username, itemId);
                return new UnauthorizedResult();
            }


            libraryItem.Id = existing.Id;
            libraryItem.UserId = existing.UserId;

            await _libraryStorage.UpdateLibraryItem(libraryItem);

            return new OkObjectResult("Item added");
        }
    }
}
