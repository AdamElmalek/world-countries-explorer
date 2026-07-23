namespace CountriesServers.Models
{
    public class UserQuizAttempt
    {
        public int AttemptId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool IsCompleted { get; set; }

        public UserQuizAttempt()
        {
            Title = string.Empty;
            StartedAt = DateTime.MinValue;
            FinishedAt = DateTime.MinValue;
        }
    }
}
