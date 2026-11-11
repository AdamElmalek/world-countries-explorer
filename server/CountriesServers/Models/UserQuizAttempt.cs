using CountriesServers.DAL;

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
        public List<UserQuizAttempt> GetByUser(int userId)
        {
            if (userId <= 0)
            {
                return new List<UserQuizAttempt>();
            }

            DBservices dbs = new DBservices();
            return dbs.GetQuizAttemptsByUser(userId);
        }

        public int GetTotalPoints(int userId)
        {
            if (userId <= 0)
            {
                return 0;
            }

            DBservices dbs = new DBservices();
            return dbs.GetTotalQuizPoints(userId);
        }
    }
}
