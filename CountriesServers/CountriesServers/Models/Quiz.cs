namespace CountriesServers.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeLimitSeconds { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionsCount { get; set; }
        public int MaximumScore { get; set; }

        public Quiz()
        {
            Title = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.MinValue;
        }
    }
}
