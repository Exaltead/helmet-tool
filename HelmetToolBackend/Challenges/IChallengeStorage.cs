using HelmetToolBackend.Models;

namespace HelmetToolBackend.Challenges;

public interface IChallengeStorage
{
    Task<string> AddChallenge(Challenge challenge);
    Task<List<Challenge>> GetChallenges();
    Task UpdateChallenge(Challenge challenge);
    Task DeleteChallenge(string id);
}