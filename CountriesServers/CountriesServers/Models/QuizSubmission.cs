namespace CountriesServers.Models
{
    public class QuizSubmission
    {
        public int AttemptId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public List<QuizAnswer> Answers { get; set; }

        public QuizSubmission()
        {
            Answers = new List<QuizAnswer>();
        }
    }
}
