namespace CountriesServers.Models
{
    public class UserLanguage
    {
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string ProficiencyLevel { get; set; }

        public UserLanguage()
        {
            LanguageCode = string.Empty;
            LanguageName = string.Empty;
            ProficiencyLevel = string.Empty;
        }

        public UserLanguage(
            string languageCode,
            string languageName,
            string proficiencyLevel)
        {
            LanguageCode = languageCode;
            LanguageName = languageName;
            ProficiencyLevel = proficiencyLevel;
        }
    }
}
