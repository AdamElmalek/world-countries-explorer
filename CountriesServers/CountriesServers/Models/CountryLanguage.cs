namespace CountriesServers.Models
{
    public class CountryLanguage
    {
        public int CountryLanguageId { get; set; }
        public int CountryId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string NativeName { get; set; }

        public CountryLanguage()
        {
            LanguageCode = string.Empty;
            LanguageName = string.Empty;
            NativeName = string.Empty;
        }

        public CountryLanguage(
            int countryLanguageId,
            int countryId,
            string languageCode,
            string languageName,
            string nativeName)
        {
            CountryLanguageId = countryLanguageId;
            CountryId = countryId;
            LanguageCode = languageCode;
            LanguageName = languageName;
            NativeName = nativeName;
        }
    }
}
