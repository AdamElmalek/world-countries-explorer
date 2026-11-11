using System.Text.Json.Serialization;

namespace CountriesServers.Models
{
    public class CountriesDevApiModels
    {
        public class CountriesDevCountry
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("nativeName")]
            public string NativeName { get; set; } = string.Empty;

            [JsonPropertyName("alpha2Code")]
            public string Alpha2Code { get; set; } = string.Empty;

            [JsonPropertyName("alpha3Code")]
            public string Alpha3Code { get; set; } = string.Empty;

            [JsonPropertyName("numericCode")]
            public string NumericCode { get; set; } = string.Empty;

            [JsonPropertyName("capital")]
            public string Capital { get; set; } = string.Empty;

            [JsonPropertyName("region")]
            public string Region { get; set; } = string.Empty;

            [JsonPropertyName("subregion")]
            public string Subregion { get; set; } = string.Empty;

            [JsonPropertyName("population")]
            public long Population { get; set; }

            [JsonPropertyName("area")]
            public decimal? Area { get; set; }

            [JsonPropertyName("latlng")]
            public List<decimal> LatLng { get; set; } = new List<decimal>();

            [JsonPropertyName("demonym")]
            public string Demonym { get; set; } = string.Empty;

            [JsonPropertyName("populationDensity")]
            public decimal? PopulationDensity { get; set; }

            [JsonPropertyName("flags")]
            public CountriesDevFlags Flags { get; set; } =
                new CountriesDevFlags();

            [JsonPropertyName("flag")]
            public string FlagEmoji { get; set; } = string.Empty;

            [JsonPropertyName("languages")]
            public List<CountriesDevLanguage> Languages { get; set; } =
                new List<CountriesDevLanguage>();

            [JsonPropertyName("currencies")]
            public List<CountriesDevCurrency> Currencies { get; set; } =
                new List<CountriesDevCurrency>();
        }


        public class CountriesDevFlags
        {
            [JsonPropertyName("svg")]
            public string Svg { get; set; } = string.Empty;

            [JsonPropertyName("png")]
            public string Png { get; set; } = string.Empty;
        }


        public class CountriesDevLanguage
        {
            [JsonPropertyName("iso639_1")]
            public string Iso6391 { get; set; } = string.Empty;

            [JsonPropertyName("iso639_2")]
            public string Iso6392 { get; set; } = string.Empty;

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("nativeName")]
            public string NativeName { get; set; } = string.Empty;
        }


        public class CountriesDevCurrency
        {
            [JsonPropertyName("code")]
            public string Code { get; set; } = string.Empty;

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("symbol")]
            public string Symbol { get; set; } = string.Empty;
        }





    }
}
