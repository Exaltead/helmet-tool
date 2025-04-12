using HelmetToolBackend.Models;

namespace HelmetToolBackend.Auth;

public interface IJwtHandler
{
    string SignUserToken(User user);
    User? VerifyAndDecode(string token);
}