namespace CountriesServers.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        public string Name { get; set; }
        public string NativeName { get; set; }

        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string NumericCode { get; set; }

        public string Capital { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }

        public long Population { get; set; }
        public decimal Area { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public string FlagUrl { get; set; }
        public string FlagPngUrl { get; set; }
        public string FlagEmoji { get; set; }

        public string Demonym { get; set; }
        public decimal PopulationDensity { get; set; }

        //string returned sp-countries-search 
        public string Languages { get; set; }
        public string Currencies { get; set; }

        //lists that used for whole country details 
        public List<CountryLanguage> LanguageList { get; set; }
        public List<CountryCurrency> CurrencyList { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Country()
        {
            Name = string.Empty;
            NativeName = string.Empty;
            Alpha2Code = string.Empty;
            Alpha3Code = string.Empty;
            NumericCode = string.Empty;
            Capital = string.Empty;
            Region = string.Empty;
            Subregion = string.Empty;
            FlagUrl = string.Empty;
            FlagPngUrl = string.Empty;
            FlagEmoji = string.Empty;
            Demonym = string.Empty;
            Languages = string.Empty;
            Currencies = string.Empty;
            LanguageList = new List<CountryLanguage>();
            CurrencyList = new List<CountryCurrency>();
            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
        }

        public Country(int countryId,string name,string nativeName,string alpha2Code,string alpha3Code,
            string numericCode,string capital,string region,string subregion,long population, decimal area,
            decimal lat,decimal lng,string flagUrl,string flagPngUrl,string flagEmoji,
            string demonym,decimal populationDensity,DateTime createdAt, DateTime updatedAt)
        {
            CountryId = countryId;

            Name = name;
            NativeName = nativeName;

            Alpha2Code = alpha2Code;
            Alpha3Code = alpha3Code;
            NumericCode = numericCode;

            Capital = capital;
            Region = region;
            Subregion = subregion;

            Population = population;
            Area = area;

            Lat = lat;
            Lng = lng;

            FlagUrl = flagUrl;
            FlagPngUrl = flagPngUrl;
            FlagEmoji = flagEmoji;

            Demonym = demonym;
            PopulationDensity = populationDensity;

            Languages = string.Empty;
            Currencies = string.Empty;

            LanguageList = new List<CountryLanguage>();
            CurrencyList = new List<CountryCurrency>();

            CreatedAt = createdAt;
            UpdatedAt = updatedAt;

        }
    }
}
