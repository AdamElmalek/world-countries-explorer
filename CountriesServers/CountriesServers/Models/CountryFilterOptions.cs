namespace CountriesServers.Models
{
    public class CountryFilterOptions
    {

        public List<string> Regions { get; set; }
        public List<CountryLanguage> Languages { get; set; }
        public List<CountryCurrency> Currencies { get; set; }

        public long MinPopulation { get; set; }
        public long MaxPopulation { get; set; }

        public decimal MinArea { get; set; }
        public decimal MaxArea { get; set; }

        public CountryFilterOptions()
        {
            Regions = new List<string>();
            Languages = new List<CountryLanguage>();
            Currencies = new List<CountryCurrency>();
        }
    }
}
