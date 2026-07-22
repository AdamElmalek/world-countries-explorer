namespace CountriesServers.Models
{
    public class UserCountryList
    {
        public int UserCountryListId { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }
        public string ListType { get; set; }
        public DateTime SavedAt { get; set; }
        public Country Country { get; set; }


        public UserCountryList()
        {
            ListType = string.Empty;
            SavedAt = DateTime.MinValue;
            Country = new Country();
        }


    }
}
