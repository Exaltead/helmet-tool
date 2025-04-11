using HelmetToolBackend.Models;

namespace HelmetToolBackend.Challenges
{
    public interface IChallengeStorage
    {
        Task<string> AddChallenge(Challenge challenge);
        Task<List<Challenge>> GetChallenges();
        Task<bool> UpdateChallenge(Challenge challenge);
        Task<bool> DeleteChallenge(string id);
    }
}