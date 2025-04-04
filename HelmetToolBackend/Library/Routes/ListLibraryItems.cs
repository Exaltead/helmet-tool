using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelmetToolBackend.Auth;
using HelmetToolBackend.Utils;

namespace HelmetToolBackend.Library.Routes
{


    public class ListLibraryItems(ILogger<AddLibraryItem> _logger, IJwtHandler _jwtHandler, ILibraryStorage _libraryStorage)
    {
        [Function("ListLibraryItems")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "library")] HttpRequest req)
        {
            var user = _jwtHandler.GetUser(req);

            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            var itemId = req.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                var item = await _libraryStorage.GetLibraryItem(itemId);
                if (item == null)
                {
                    _logger.LogWarning("Library item with id {id} not found.", itemId);
                    return new NotFoundResult();
                }
                return new OkObjectResult(new[] { item });
            }

            var items = await _libraryStorage.GetAllLibraryItems(user.Id);

            return new OkObjectResult(items);
        }
    }
}
