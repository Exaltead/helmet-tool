using HelmetToolBackend.Models;

namespace HelmetToolBackend.Library
{
    public interface ILibraryStorage
    {
        Task AddLibraryItem(LibraryItem item);
        Task DeleteLibraryItem(string id);
        Task<LibraryItem?> GetLibraryItem(string id);
        Task<List<LibraryItem>> GetAllLibraryItems(string userId);
    }
}