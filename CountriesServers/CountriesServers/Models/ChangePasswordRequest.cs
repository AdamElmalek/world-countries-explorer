namespace CountriesServers.Models
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordRequest()
        {
            Email = string.Empty;
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
        }
    }
}
