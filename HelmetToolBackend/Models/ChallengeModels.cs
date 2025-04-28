using HelmetToolBackend.Shared;

namespace HelmetToolBackend.Models
{
    public record QuestionRecord
    {
        public string Kind { get; set; } = "Boolean";
        public string Id { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public int Number { get; set; } = 0;

        public int QuestionClusterSize { get; set; } = 1;
    }


    public record Challenge : IDbEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public string TargetMedia { get; set; } = string.Empty;

        public QuestionRecord[] Questions { get; set; } = [];
    }

    public record AnswerRecord
    {
        public string Kind { get; set; } = "Boolean";
        public string Id { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public bool Answered { get; set; } = false;
        public string Answer { get; set; } = string.Empty;

    }

    public record ChallengeAnswerSet : IDbEntity
    {
        public string Id { get; set; } = string.Empty;
        public string ChallengeId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset AddDate { get; set; } = DateTimeOffset.UtcNow;
        public AnswerRecord[] Answers { get; set; } = [];
    }
}
