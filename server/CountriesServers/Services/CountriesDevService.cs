using CountriesServers.Models;
using System.Text.Json;
using static CountriesServers.Models.CountriesDevApiModels;

namespace CountriesServers.Services
{
    public class CountriesDevService
    {
        private static readonly HttpClient httpClient = new HttpClient();
           


        public async Task<int> ImportCountries()
        {
            string fields =
                "name,nativeName,alpha2Code,alpha3Code,numericCode," +
                "capital,region,subregion,population,area,latlng," +
                "demonym,populationDensity,flags,flag,languages,currencies";

            string url ="https://countries.dev/countries?fields=" + fields;
                

            HttpResponseMessage response = await httpClient.GetAsync(url);
                

            string json =await response.Content.ReadAsStringAsync();
                

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("countries.dev request failed. Status code: " +(int)response.StatusCode);
                                     
            }

            List<CountriesDevCountry>? apiCountries =
                JsonSerializer.Deserialize<List<CountriesDevCountry>>(json,new JsonSerializerOptions
                    
                    
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            if (apiCountries == null)
            {
                return 0;
            }

            int savedCountries = 0;

            foreach (CountriesDevCountry apiCountry in apiCountries)
            {
                Country country = ConvertToCountry(apiCountry);

                if (!IsValidCountry(country))
                {
                    continue;
                }

                int countryId = country.SaveImportedCountry();

                if (countryId > 0)
                {
                    savedCountries++;
                }
            }

            return savedCountries;
        }


        private Country ConvertToCountry(CountriesDevCountry apiCountry)
            
        {
            Country country = new Country();

            country.Name = apiCountry.Name.Trim();
            country.NativeName = apiCountry.NativeName.Trim();

            country.Alpha2Code = apiCountry.Alpha2Code.Trim().ToUpper();
                

            country.Alpha3Code = apiCountry.Alpha3Code.Trim().ToUpper();
                

            country.NumericCode = apiCountry.NumericCode.Trim();
                

            country.Capital = apiCountry.Capital.Trim();
            country.Region = apiCountry.Region.Trim();
            country.Subregion = apiCountry.Subregion.Trim();

            country.Population = apiCountry.Population;

            if (apiCountry.Area.HasValue)
            {
                country.Area = apiCountry.Area.Value;
            }

            if (apiCountry.LatLng.Count >= 2)
            {
                country.Lat = apiCountry.LatLng[0];
                country.Lng = apiCountry.LatLng[1];
            }

            country.Demonym = apiCountry.Demonym.Trim();

            if (apiCountry.PopulationDensity.HasValue)
            {
                country.PopulationDensity =apiCountry.PopulationDensity.Value;
                    
            }
            else if (country.Area > 0)
            {
                country.PopulationDensity =country.Population / country.Area;
                    
            }

            country.FlagUrl = apiCountry.Flags.Svg.Trim();
            country.FlagPngUrl = apiCountry.Flags.Png.Trim();
            country.FlagEmoji = apiCountry.FlagEmoji.Trim();

            country.LanguageList =ConvertLanguages(apiCountry.Languages);
                

            country.CurrencyList = ConvertCurrencies(apiCountry.Currencies);
               

            return country;
        }


        private List<CountryLanguage> ConvertLanguages(List<CountriesDevLanguage> apiLanguages)
            
        {
            List<CountryLanguage> languages = new List<CountryLanguage>();
               

            foreach (CountriesDevLanguage apiLanguage in apiLanguages)
            {
                string languageCode = apiLanguage.Iso6391.Trim().ToLower();
                   

                if (languageCode == string.Empty)
                {
                    languageCode =apiLanguage.Iso6392.Trim().ToLower();
                        
                }

                string languageName = apiLanguage.Name.Trim();
                   

                if (languageCode == string.Empty ||languageName == string.Empty)
                    
                {
                    continue;
                }

                bool alreadyExists =languages.Any(language =>language.LanguageCode == languageCode);

                if (alreadyExists)
                {
                    continue;
                }

                CountryLanguage language = new CountryLanguage();
                   

                language.LanguageCode = languageCode;
                language.LanguageName = languageName;
                language.NativeName =apiLanguage.NativeName.Trim();

                languages.Add(language);
            }

            return languages;
        }


        private List<CountryCurrency> ConvertCurrencies(
            List<CountriesDevCurrency> apiCurrencies)
        {
            List<CountryCurrency> currencies = new List<CountryCurrency>();
               

            foreach (CountriesDevCurrency apiCurrency in apiCurrencies)
            {
                string currencyCode =apiCurrency.Code.Trim().ToUpper();

                string currencyName =apiCurrency.Name.Trim();

                if (currencyCode == string.Empty ||currencyName == string.Empty)
                {
                    continue;
                }

                bool alreadyExists =currencies.Any(currency =>currency.CurrencyCode == currencyCode);

                if (alreadyExists)
                {
                    continue;
                }

                CountryCurrency currency = new CountryCurrency();
                   

                currency.CurrencyCode = currencyCode;
                currency.CurrencyName = currencyName;
                currency.CurrencySymbol =apiCurrency.Symbol.Trim();

                currencies.Add(currency);
            }

            return currencies;
        }


        private bool IsValidCountry(Country country)
        {
            if (country.Name == string.Empty)
            {
                return false;
            }

            if (country.Alpha2Code.Length != 2)
            {
                return false;
            }

            if (country.Alpha3Code.Length != 3)
            {
                return false;
            }

            if (country.Population < 0)
            {
                return false;
            }

            return true;
        }


    }
}
