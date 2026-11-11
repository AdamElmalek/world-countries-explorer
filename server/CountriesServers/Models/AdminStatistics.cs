namespace CountriesServers.Models
{
    public class AdminStatistics
    {
        public int UsersCount { get; set; }
        public int LockedUsersCount { get; set; }
        public int BlockedSharingCount { get; set; }
        public int CountriesCount { get; set; }
        public int SharesCount { get; set; }
        public int SavedCountriesCount { get; set; }
        public int CompletedQuizAttemptsCount { get; set; }
        public int LoginsToday { get; set; }

    }
}
