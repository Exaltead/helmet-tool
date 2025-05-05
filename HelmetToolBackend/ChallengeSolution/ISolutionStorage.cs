using HelmetToolBackend.Models;

namespace HelmetToolBackend.ChallengeSolution;

public interface ISolutionStorage
{
    Task<string> AddSolutionSet(SolutionSet solution);
    Task<List<SolutionSet>> GetSolutionSets(string userId);
    Task UpdateSolutionSet(SolutionSet solution);
    Task DeleteSolutionSet(string id, string userId);
    Task<SolutionSet?> GetSolutionSet(string id, string userId);
    Task<SolutionSet?> GetSolutionSetByChallengeId(string challengeId, string userId);
}