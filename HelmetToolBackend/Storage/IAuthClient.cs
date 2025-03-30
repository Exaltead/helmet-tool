namespace HelmetToolBackend.Storage
{
    public interface IAuthClient
    {
        Task<bool> ValidLogin(string username, string password);
    }
}