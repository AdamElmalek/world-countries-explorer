namespace CountriesServers.Models
{
    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public int Points { get; set; }

        public QuizQuestion()
        {
            QuestionText = string.Empty;
            OptionA = string.Empty;
            OptionB = string.Empty;
            OptionC = string.Empty;
            OptionD = string.Empty;
        }
    }
}
