namespace CountriesServers.Models
{
    public class QuizCorrectAnswer
    {
        public int QuestionId { get; set; }
        public string CorrectOption { get; set; }
        public int Points { get; set; }

        public QuizCorrectAnswer()
        {
            CorrectOption = string.Empty;
        }
    }
}
