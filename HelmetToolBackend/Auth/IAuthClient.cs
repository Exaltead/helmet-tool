using HelmetToolBackend.Models;

namespace HelmetToolBackend.Auth;




public interface IAuthClient
{
    Task<User?> ValidLogin(string username, string password);
}