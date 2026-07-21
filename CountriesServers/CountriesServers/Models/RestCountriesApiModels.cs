using System.Text.Json;
using System.Text.Json.Serialization;

namespace CountriesServers.Models
{
   
        public class RestCountriesResponse
        {
            [JsonPropertyName("data")]
            public RestCountriesData Data { get; set; } = new RestCountriesData();
        }


        public class RestCountriesData
        {
            [JsonPropertyName("objects")]
            public List<RestCountryApi> Objects { get; set; } =
                new List<RestCountryApi>();

            [JsonPropertyName("meta")]
            public RestCountriesMeta Meta { get; set; } =
                new RestCountriesMeta();
        }


        public class RestCountriesMeta
        {
            [JsonPropertyName("total")]
            public int Total { get; set; }

            [JsonPropertyName("count")]
            public int Count { get; set; }

            [JsonPropertyName("limit")]
            public int Limit { get; set; }

            [JsonPropertyName("offset")]
            public int Offset { get; set; }

            [JsonPropertyName("more")]
            public bool More { get; set; }
        }


        public class RestCountryApi
        {
            [JsonPropertyName("names")]
            public RestCountryNames Names { get; set; } =
                new RestCountryNames();

            [JsonPropertyName("codes")]
            public RestCountryCodes Codes { get; set; } =
                new RestCountryCodes();

            /*
             * Capitals are an array of objects in API v5.
             * We will read the first capital later during mapping.
             */
            [JsonPropertyName("capitals")]
            public JsonElement Capitals { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; } = string.Empty;

            [JsonPropertyName("subregion")]
            public string Subregion { get; set; } = string.Empty;

            [JsonPropertyName("population")]
            public long Population { get; set; }

            [JsonPropertyName("area")]
            public RestCountryArea Area { get; set; } =
                new RestCountryArea();

            [JsonPropertyName("coordinates")]
            public RestCountryCoordinates Coordinates { get; set; } =
                new RestCountryCoordinates();

            [JsonPropertyName("flag")]
            public RestCountryFlag Flag { get; set; } =
                new RestCountryFlag();

            /*
             * Languages are returned as an array.
             * JsonElement lets us safely read all available ISO fields.
             */
            [JsonPropertyName("languages")]
            public JsonElement Languages { get; set; }

            /*
             * Currencies are returned as an object whose keys are
             * currency codes such as USD, EUR and ILS.
             */
            [JsonPropertyName("currencies")]
            public JsonElement Currencies { get; set; }

            /*
             * Demonyms are keyed by language code.
             */
            [JsonPropertyName("demonyms")]
            public JsonElement Demonyms { get; set; }
        }


        public class RestCountryNames
        {
            [JsonPropertyName("common")]
            public string Common { get; set; } = string.Empty;

            [JsonPropertyName("official")]
            public string Official { get; set; } = string.Empty;

            /*
             * Native names are keyed by language code.
             */
            [JsonPropertyName("native")]
            public JsonElement Native { get; set; }
        }


        public class RestCountryCodes
        {
            [JsonPropertyName("alpha_2")]
            public string Alpha2 { get; set; } = string.Empty;

            [JsonPropertyName("alpha_3")]
            public string Alpha3 { get; set; } = string.Empty;

            [JsonPropertyName("ccn3")]
            public string NumericCode { get; set; } = string.Empty;
        }


        public class RestCountryArea
        {
            [JsonPropertyName("kilometers")]
            public decimal Kilometers { get; set; }

            [JsonPropertyName("miles")]
            public decimal Miles { get; set; }
        }


        public class RestCountryCoordinates
        {
            [JsonPropertyName("lat")]
            public decimal Lat { get; set; }

            [JsonPropertyName("lng")]
            public decimal Lng { get; set; }
        }


        public class RestCountryFlag
        {
            [JsonPropertyName("emoji")]
            public string Emoji { get; set; } = string.Empty;

            [JsonPropertyName("url_png")]
            public string PngUrl { get; set; } = string.Empty;

            [JsonPropertyName("url_svg")]
            public string SvgUrl { get; set; } = string.Empty;
        }




    
}
