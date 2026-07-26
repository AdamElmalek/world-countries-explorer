using CountriesServers.DAL;

namespace CountriesServers.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        public bool IsAdmin()
        {
            if (AdminId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();
            return dbs.IsAdmin(AdminId);
        }

        public List<User> GetUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.AdminGetUsers();
        }

        public bool SetUserLocked(int userId, bool isLocked)
        {
            if (userId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();
            return dbs.AdminSetUserLocked(userId, isLocked);
        }

        public bool SetUserCanShare(int userId, bool canShare)
        {
            if (userId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();
            return dbs.AdminSetUserCanShare(userId, canShare);
        }

        public bool DeleteShare(int shareId)
        {
            if (shareId <= 0)
            {
                return false;
            }

            DBservices dbs = new DBservices();
            return dbs.AdminDeleteShare(shareId);
        }

        public AdminStatistics GetStatistics()
        {
            DBservices dbs = new DBservices();
            return dbs.AdminGetStatistics();
        }
    }
}
