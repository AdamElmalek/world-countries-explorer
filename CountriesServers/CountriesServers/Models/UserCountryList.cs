using CountriesServers.DAL;

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
        public UserCountryList(int userId, int countryId, string listType)
        {
            UserId = userId;
            CountryId = countryId;
            ListType = listType;

            SavedAt = DateTime.MinValue;
            Country = new Country();
        }
        // Add a country to Visited or WantToVisit.
        public int Add()
        {
            if (UserId <= 0 ||CountryId <= 0 ||!IsValidListType(ListType))                           
            {
                return 0;
            }

            DBservices dbs = new DBservices();

            return dbs.AddCountryToList(UserId,CountryId,ListType);
                             
                           
       }


        // Get all countries from one of the user's lists.
        public List<UserCountryList> GetByType(int userId,string listType)            
        {
            if (userId <= 0 ||!IsValidListType(listType))
                
            {
                return new List<UserCountryList>();
            }

            DBservices dbs = new DBservices();

            return dbs.GetCountriesByListType(userId,listType);
            
        }


        // Move a saved country to the other list.
        public bool Move(string toType)
        {
            if (UserId <= 0 ||CountryId <= 0 ||!IsValidListType(toType))                         
            {
                return false;
            }

            DBservices dbs = new DBservices();

            return dbs.MoveCountryToList(UserId,CountryId,toType );
        }

        // Remove a country from the user's personal list.
        public bool Remove()
        {
            if (UserId <= 0 || CountryId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();

            return dbs.RemoveCountryFromList(UserId,CountryId );
        }

        // Check whether the country is saved and in which list.
        public UserCountryList? GetStatus()
        {
            if (UserId <= 0 || CountryId <= 0)
            {
                return null;
            }

            DBservices dbs = new DBservices();

            return dbs.GetCountryListStatus(UserId,CountryId);
        }
        private bool IsValidListType(string listType)
        {
            return listType == "Visited" ||listType == "WantToVisit";
                   
        }

    }
}
