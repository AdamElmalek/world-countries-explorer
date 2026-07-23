namespace CountriesServers.Models
{
    public class QuizResult
    {
        public int AttemptId { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalPoints { get; set; }
        public bool TimeExpired { get; set; }

        public QuizResult()
        {

        }
    }
}
