namespace HelmetToolBackend.Models
{
    public record LibraryBook
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Translator { get; set; }
        public string? Id { get; set; } = string.Empty;
    }

    public record LibraryItem
    {
        public LibraryBook? Book { get; set; }
        public string? Id { get; set; }

        public string? UserId { get; set; }
        public DateTimeOffset AddDate { get; set; } = DateTimeOffset.UtcNow;
    }
}