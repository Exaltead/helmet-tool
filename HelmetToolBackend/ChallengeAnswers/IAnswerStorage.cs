using HelmetToolBackend.Models;

namespace HelmetToolBackend.ChallengeAnswers;

public interface IAnswerStorage
{
    Task<string> AddAnswerSet(ChallengeAnswerSet answers);
    Task<List<ChallengeAnswerSet>> GetAnswers(string userId, string challengeId, string itemId);
    Task UpdateAnswers(ChallengeAnswerSet answer);
    Task DeleteAnswer(string id, string userId);

    Task<ChallengeAnswerSet?> GetAnswer(string id, string userId);
}