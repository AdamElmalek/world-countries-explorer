namespace CountriesServers.Models
{
    public class QuizAnswer
    {
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }

        public QuizAnswer()
        {
            SelectedOption = string.Empty;
        }
    }
}
