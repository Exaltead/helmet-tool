using HelmetToolBackend.Models;

namespace HelmetToolBackend.Library
{
    public interface ILibraryStorage
    {
        Task<string> AddLibraryItem(LibraryItem item);
        Task DeleteLibraryItem(string id);
        Task<LibraryItem?> GetLibraryItem(string id, string userId);
        Task<List<LibraryItem>> GetAllLibraryItems(string userId);

        Task UpdateLibraryItem(LibraryItem item);
    }
}