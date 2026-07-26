namespace CountriesServers.Models
{
    public class AIAnswerResponse
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }
}
