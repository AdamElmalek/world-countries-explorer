using CountriesServers.DAL;

namespace CountriesServers.Models
{
    public class Share
    {
        public int ShareId { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string UserName { get; set; }

        public string CountryName { get; set; }
        public string Alpha3Code { get; set; }
        public string FlagUrl { get; set; }
        public string FlagPngUrl { get; set; }

        public Share()
        {
            Content = string.Empty;
            UserName = string.Empty;
            CountryName = string.Empty;
            Alpha3Code = string.Empty;
            FlagUrl = string.Empty;
            FlagPngUrl = string.Empty;

            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
        }

        public Share(int userId, int countryId, string content)
        {
            UserId = userId;
            CountryId = countryId;
            Content = content;

            UserName = string.Empty;
            CountryName = string.Empty;
            Alpha3Code = string.Empty;
            FlagUrl = string.Empty;
            FlagPngUrl = string.Empty;

            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
        }

        // Add a new share.
        public int Add()
        {
            Content = Content.Trim();

            if (UserId <= 0 || CountryId <= 0 || !IsValidContent())
            {
                return 0;
            }

            DBservices dbs = new DBservices();
            return dbs.AddShare(this);
        }

        // Get all shares in the website.
        public List<Share> GetAll()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllShares();
        }

        // Get all shares related to one country.
        public List<Share> GetByCountry(int countryId)
        {
            if (countryId <= 0)
            {
                return new List<Share>();
            }

            DBservices dbs = new DBservices();
            return dbs.GetSharesByCountry(countryId);
        }

        // Update a share created by this user.
        public int Update()
        {
            Content = Content.Trim();

            if (ShareId <= 0 || UserId <= 0 || !IsValidContent())
            {
                return 0;
            }

            DBservices dbs = new DBservices();
            return dbs.UpdateShare(this);
        }

        // Delete a share created by this user.
        public bool Delete()
        {
            if (ShareId <= 0 || UserId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();
            return dbs.DeleteShare(ShareId, UserId);
        }

        private bool IsValidContent()
        {
            return Content.Length >= 1 && Content.Length <= 1000;
        }


    }
}
