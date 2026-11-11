namespace CountriesServers.Models
{
    public class AIQuestionRequest
    {
        public int UserId { get; set; }
        public int CountryId { get; set; }
        public string Question { get; set; } = string.Empty;
    }
}
