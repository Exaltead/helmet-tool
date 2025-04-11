namespace HelmetToolBackend.Models
{
    public record QuestionRecord
    {
        public string Id { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public int Number { get; set; } = 0;
    }


    public record Challenge
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public string TargetMedia { get; set; } = string.Empty;

        public QuestionRecord[] Questions { get; set; } = [];
    }
}
