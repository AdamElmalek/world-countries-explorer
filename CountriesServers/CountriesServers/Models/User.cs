using CountriesServers.DAL;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace CountriesServers.Models
{
    public class User
    {
        // =====================================================
        // Properties
        // =====================================================

        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }//passhash stored in sql it never retrun in json response
        public string Role { get; set; }
        public bool IsLocked { get; set; }
        public bool CanShare { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime LastLoginAt { get; set; }

        // =====================================================
        // Constructors
        // =====================================================

        /*
         * empty constructor when it converts
         * JSON received from the Client into a User object.
         */
        public User()
        {
            UserId = 0;
            FullName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            PasswordHash = string.Empty;
            Role = "User";
            IsLocked = false;
            CanShare = true;
            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
            LastLoginAt = DateTime.MinValue;
        }

        /*
         * Useful when creating a User for registration.
         */
        public User(
            string fullName,
            string email,
            string password)
        {
            UserId = 0;
            FullName = fullName;
            Email = email;
            Password = password;
            PasswordHash = string.Empty;
            Role = "User";
            IsLocked = false;
            CanShare = true;
            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
            LastLoginAt = DateTime.MinValue;
        }

        /*
         * Useful when DBservices reads a complete user
         * from SP_Users_GetByEmail.
         */
        public User(
            int userId,
            string fullName,
            string email,
            string passwordHash,
            string role,
            bool isLocked,
            bool canShare,
            DateTime createdAt,
            DateTime updatedAt,
            DateTime lastLoginAt)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            Password = string.Empty;
            PasswordHash = passwordHash;
            Role = role;
            IsLocked = isLocked;
            CanShare = canShare;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            LastLoginAt = lastLoginAt;
        }

        // =====================================================
        // Register
        // =====================================================

        /*
         * Returns:
         *
         * positive number = new UserId
         * -1              = email already exists
         * 0               = invalid information or failure
         */
        public int Register()
        {
            FullName = NormalizeText(FullName);
            Email = NormalizeEmail(Email);

            if (FullName == string.Empty ||
                Email == string.Empty ||
                Password == string.Empty)
            {
                return 0;
            }

            if (!IsValidEmail(Email))
            {
                return 0;
            }

            if (Password.Length < 6)
            {
                return 0;
            }

            PasswordHash = HashPassword(Password);

            /*
             * These values are controlled by the Server.
             * The Client cannot register itself as Admin.
             */
            Role = "User";
            IsLocked = false;
            CanShare = true;

            DBservices dbs = new DBservices();

            int result = dbs.RegisterUser(this);

            /*
             * Remove the plain password after using it.
             */
            Password = string.Empty;

            return result;
        }

        // =====================================================
        // Login
        // =====================================================

        public User? Login()
        {
            Email = NormalizeEmail(Email);

            if (Email == string.Empty ||
                Password == string.Empty)
            {
                return null;
            }

            DBservices dbs = new DBservices();

            /*
             * SQL searches only by email and returns
             * the stored PasswordHash.
             */
            User? storedUser = dbs.GetUserByEmail(Email);

            if (storedUser == null)
            {
                Password = string.Empty;
                return null;
            }

            if (storedUser.PasswordHash == string.Empty)
            {
                Password = string.Empty;
                return null;
            }

            bool passwordIsCorrect = VerifyPassword(
                Password,
                storedUser.PasswordHash
            );

            Password = string.Empty;

            if (!passwordIsCorrect)
            {
                return null;
            }

            /*
             * Return the locked user to the Controller.
             * The Controller will return HTTP 403.
             */
            if (storedUser.IsLocked)
            {
                storedUser.Password = string.Empty;
                storedUser.PasswordHash = string.Empty;

                return storedUser;
            }

            /*
             * Recording the login is secondary.
             * A logging failure must not reject a valid login.
             */
            try
            {
                bool loginRecorded =
                    dbs.LogUserLogin(storedUser.UserId);

                if (loginRecorded)
                {
                    storedUser.LastLoginAt = DateTime.Now;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "The login succeeded, but it could not be logged: "
                    + exception.Message
                );
            }

            storedUser.Password = string.Empty;
            storedUser.PasswordHash = string.Empty;

            return storedUser;
        }

        // =====================================================
        // Get profile
        // =====================================================

        public User? GetById(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            DBservices dbs = new DBservices();

            return dbs.GetUserById(userId);
        }

        // =====================================================
        // Update profile
        // =====================================================

        /*
         * Returns:
         *
         *  1 = updated
         *  0 = user not found or invalid information
         * -1 = email already belongs to another user
         */
        public int UpdateProfile()
        {
            FullName = NormalizeText(FullName);
            Email = NormalizeEmail(Email);

            if (UserId <= 0 ||
                FullName == string.Empty ||
                Email == string.Empty)
            {
                return 0;
            }

            if (!IsValidEmail(Email))
            {
                return 0;
            }

            DBservices dbs = new DBservices();

            return dbs.UpdateUserProfile(this);
        }

        // =====================================================
        // Change password
        // =====================================================

        public bool ChangePassword(
            string currentPassword,
            string newPassword)
        {
            Email = NormalizeEmail(Email);

            if (UserId <= 0 ||
                Email == string.Empty ||
                currentPassword == string.Empty ||
                newPassword == string.Empty)
            {
                return false;
            }

            if (newPassword.Length < 6)
            {
                return false;
            }

            DBservices dbs = new DBservices();

            /*
             * Get the existing hash using the user's email.
             */
            User? storedUser =
                dbs.GetUserByEmail(Email);

            if (storedUser == null)
            {
                return false;
            }

            /*
             * Prevent changing another user's password.
             */
            if (storedUser.UserId != UserId)
            {
                return false;
            }

            if (storedUser.IsLocked)
            {
                return false;
            }

            bool currentPasswordIsCorrect =
                VerifyPassword(
                    currentPassword,
                    storedUser.PasswordHash
                );

            if (!currentPasswordIsCorrect)
            {
                return false;
            }

            string newPasswordHash =
                HashPassword(newPassword);

            return dbs.UpdateUserPassword(
                UserId,
                newPasswordHash
            );
        }

        // =====================================================
        // Continent preferences
        // =====================================================

        public List<string> GetContinents()
        {
            if (UserId <= 0)
            {
                return new List<string>();
            }

            DBservices dbs = new DBservices();

            return dbs.GetUserContinents(UserId);
        }

        /*
         * Returns:
         *
         * positive number = new UserContinentId
         * -1              = duplicate
         * 0               = invalid information or failure
         */
        public int AddContinent(string continentName)
        {
            continentName = NormalizeText(continentName);

            if (UserId <= 0 ||
                continentName == string.Empty)
            {
                return 0;
            }

            if (!IsValidContinent(continentName))
            {
                return 0;
            }

            DBservices dbs = new DBservices();

            return dbs.AddUserContinent(
                UserId,
                continentName
            );
        }

        public bool RemoveContinent(string continentName)
        {
            continentName = NormalizeText(continentName);

            if (UserId <= 0 ||
                continentName == string.Empty)
            {
                return false;
            }

            if (!IsValidContinent(continentName))
            {
                return false;
            }

            DBservices dbs = new DBservices();

            return dbs.RemoveUserContinent(
                UserId,
                continentName
            );
        }

        // =====================================================
        // Language preferences
        // =====================================================

        public List<UserLanguage> GetLanguages()
        {
            if (UserId <= 0)
            {
                return new List<UserLanguage>();
            }

            DBservices dbs = new DBservices();

            return dbs.GetUserLanguages(UserId);
        }

        /*
         * Returns:
         *
         * positive number = new UserLanguageId
         * -1              = duplicate
         * 0               = invalid information or failure
         */
        public int AddLanguage(UserLanguage language)
        {
            if (UserId <= 0 ||
                language == null)
            {
                return 0;
            }

            language.LanguageCode =
                NormalizeLanguageCode(language.LanguageCode);

            language.LanguageName =
                NormalizeText(language.LanguageName);

            language.ProficiencyLevel =
                NormalizeText(language.ProficiencyLevel);

            if (language.LanguageCode == string.Empty ||
                language.LanguageName == string.Empty ||
                language.ProficiencyLevel == string.Empty)
            {
                return 0;
            }

            if (!IsValidProficiencyLevel(
                    language.ProficiencyLevel))
            {
                return 0;
            }

            DBservices dbs = new DBservices();

            return dbs.AddUserLanguage(
                UserId,
                language
            );
        }

        public bool UpdateLanguage(
            string languageCode,
            string proficiencyLevel)
        {
            languageCode =
                NormalizeLanguageCode(languageCode);

            proficiencyLevel =
                NormalizeText(proficiencyLevel);

            if (UserId <= 0 ||
                languageCode == string.Empty ||
                proficiencyLevel == string.Empty)
            {
                return false;
            }

            if (!IsValidProficiencyLevel(
                    proficiencyLevel))
            {
                return false;
            }

            DBservices dbs = new DBservices();

            return dbs.UpdateUserLanguage(
                UserId,
                languageCode,
                proficiencyLevel
            );
        }

        public bool RemoveLanguage(string languageCode)
        {
            languageCode =
                NormalizeLanguageCode(languageCode);

            if (UserId <= 0 ||
                languageCode == string.Empty)
            {
                return false;
            }

            DBservices dbs = new DBservices();

            return dbs.RemoveUserLanguage(
                UserId,
                languageCode
            );
        }

        // =====================================================
        // Password security
        // =====================================================

        public static string HashPassword(string password)
        {
            const int iterations = 100000;
            const int saltSize = 16;
            const int hashSize = 32;

            byte[] salt =
                RandomNumberGenerator.GetBytes(saltSize);

            byte[] hash =
                Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    hashSize
                );

            string passwordHash =
                iterations + "." +
                Convert.ToBase64String(salt) + "." +
                Convert.ToBase64String(hash);

            return passwordHash;
        }

        public static bool VerifyPassword(
            string plainPassword,
            string passwordHash)
        {
            try
            {
                string[] parts =
                    passwordHash.Split('.');

                if (parts.Length != 3)
                {
                    return false;
                }

                int iterations =
                    Convert.ToInt32(parts[0]);

                byte[] salt =
                    Convert.FromBase64String(parts[1]);

                byte[] expectedHash =
                    Convert.FromBase64String(parts[2]);

                byte[] actualHash =
                    Rfc2898DeriveBytes.Pbkdf2(
                        plainPassword,
                        salt,
                        iterations,
                        HashAlgorithmName.SHA256,
                        expectedHash.Length
                    );

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash
                );
            }
            catch
            {
                return false;
            }
        }

        // =====================================================
        // Validation helpers
        // =====================================================

        private static string NormalizeText(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Trim();
        }

        private static string NormalizeEmail(string email)
        {
            if (email == null)
            {
                return string.Empty;
            }

            return email
                .Trim()
                .ToLowerInvariant();
        }

        private static string NormalizeLanguageCode(
            string languageCode)
        {
            if (languageCode == null)
            {
                return string.Empty;
            }

            return languageCode
                .Trim()
                .ToLowerInvariant();
        }

        private static bool IsValidEmail(string email)
        {
            if (email == string.Empty)
            {
                return false;
            }

            try
            {
                MailAddress address =
                    new MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidProficiencyLevel(
            string proficiencyLevel)
        {
            return
                proficiencyLevel == "Beginner" ||
                proficiencyLevel == "Intermediate" ||
                proficiencyLevel == "Advanced" ||
                proficiencyLevel == "Native";
        }

        private static bool IsValidContinent(string continentName)
        {
            return
                continentName == "Africa" ||
                continentName == "Americas" ||
                continentName == "Asia" ||
                continentName == "Europe" ||
                continentName == "Oceania" ||
                continentName == "Polar" ||
                continentName == "Antarctic Ocean" ||
                continentName == "Antarctic";
        }

    }
}
