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

    public class DeleteLibraryItem(ILogger<AddLibraryItem> _logger, IJwtHandler _jwtHandler, ILibraryStorage _libraryStorage)
    {
        [Function("DeleteLibraryItem")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "library/{itemId}")] HttpRequest req, string itemId)
        {
            var user = _jwtHandler.GetUser(req);

            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            _logger.LogInformation("User {user} tried to delete a library item {itemId}.", user?.Username, itemId);

            var item = await _libraryStorage.GetLibraryItem(itemId);
            if (item == null)
            {
                _logger.LogWarning("Library item with id {id} not found.", itemId);
                return new NotFoundResult();
            }

            if (item.UserId != user!.Id)
            {
                _logger.LogWarning("User {user} tried to delete a library item {itemId} that does not belong to them.", user?.Username, itemId);
                return new UnauthorizedResult();
            }

            await _libraryStorage.DeleteLibraryItem(itemId);
            _logger.LogInformation("Library item {itemId} deleted.", itemId);

            return new OkObjectResult("Library item deleted successfully.");
        }
    }
}
