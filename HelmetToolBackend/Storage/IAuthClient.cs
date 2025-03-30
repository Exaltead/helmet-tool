namespace HelmetToolBackend.Storage
{

    public record User(string Id, string Username);

    public interface IAuthClient
    {
        Task<User?> ValidLogin(string username, string password);
    }
}