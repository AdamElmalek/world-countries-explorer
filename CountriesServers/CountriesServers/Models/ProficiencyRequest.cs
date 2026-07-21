namespace CountriesServers.Models
{
    public class ProficiencyRequest
    {
        public string ProficiencyLevel { get; set; }

        public ProficiencyRequest()
        {
            ProficiencyLevel = string.Empty;
        }
    }
}
