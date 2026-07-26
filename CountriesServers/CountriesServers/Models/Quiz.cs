using CountriesServers.DAL;

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
        public List<Quiz> GetActive()
        {
            DBservices dbs = new DBservices();
            return dbs.GetActiveQuizzes();
        }

        public List<QuizQuestion> GetQuestions()
        {
            if (QuizId <= 0)
            {
                return new List<QuizQuestion>();
            }

            DBservices dbs = new DBservices();
            return dbs.GetQuizQuestions(QuizId);
        }

        public int StartAttempt(int userId)
        {
            if (userId <= 0 || QuizId <= 0)
            {
                return 0;
            }

            DBservices dbs = new DBservices();
            return dbs.StartQuizAttempt(userId, QuizId);
        }

    }
}
