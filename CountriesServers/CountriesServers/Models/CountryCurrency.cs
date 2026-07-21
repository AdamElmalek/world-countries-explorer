namespace CountriesServers.Models
{
    public class CountryCurrency
    {
         public int CountryCurrencyId { get; set; }
        public int CountryId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }

        public CountryCurrency()
        {
            CurrencyCode = string.Empty;
            CurrencyName = string.Empty;
            CurrencySymbol = string.Empty;
        }

        public CountryCurrency(
            int countryCurrencyId,
            int countryId,
            string currencyCode,
            string currencyName,
            string currencySymbol)
        {
            CountryCurrencyId = countryCurrencyId;
            CountryId = countryId;
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
            CurrencySymbol = currencySymbol;
        }
    }

    
}
