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


    public class AddLibraryItem(ILogger<AddLibraryItem> _logger, IJwtHandler _jwtHandler, ILibraryStorage _libraryStorage)
    {


        [Function("AddLibraryItem")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "library")] HttpRequest req)
        {
            var user = _jwtHandler.GetUser(req);

            if (user == null)
            {
                _logger.LogWarning("User not authenticated.");
                return new UnauthorizedResult();
            }

            _logger.LogInformation("User {user} tried to add a library item.", user?.Username);

            var libraryItem = await req.GetBody<LibraryItem>();

            if (libraryItem == null)
            {
                _logger.LogWarning("Library item is null.");
                return new BadRequestObjectResult("Library item is null.");
            }

            var newItem = libraryItem with { UserId = user!.Id, Id = Guid.NewGuid().ToString() };
            if (newItem.Book != null)
            {
                newItem.Book.Id = newItem.Id;
            }


            await _libraryStorage.AddLibraryItem(newItem);

            return new OkObjectResult("Item added");
        }
    }
}
