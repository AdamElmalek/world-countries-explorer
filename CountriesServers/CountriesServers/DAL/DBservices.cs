using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public class DBservices
    {
        public DBservices()
        {
        }

        // =====================================================
        // Connect
        // =====================================================

        public SqlConnection connect(string conString)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var connectionString =
                configuration.GetConnectionString(conString);

            if (connectionString == null)
            {
                throw new Exception(
                    "Connection string '" +
                    conString +
                    "' was not found."
                );
            }

            SqlConnection con =
                new SqlConnection(connectionString);

            con.Open();

            return con;
        }

        // =====================================================
        // 1. Register user
        // SP_Users_Register
        //
        // Returns:
        // positive number = new UserId
        // -1              = duplicate email
        // 0               = no result
        // =====================================================

        public int RegisterUser(User user)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@FullName", user.FullName);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@PasswordHash", user.PasswordHash);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_Register",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 2. Get user by email
        // SP_Users_GetByEmail
        //
        // PasswordHash is returned because it is needed
        // for password verification during login.
        // =====================================================

        public User? GetUserByEmail(string email)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@Email", email);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_GetByEmail",
                con,
                paramDic
            );

            try
            {
                using SqlDataReader dataReader =
                    cmd.ExecuteReader();

                /*
                 * Email is unique, so at most one row
                 * can be returned.
                 */
                if (dataReader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(
                            dataReader["UserId"]
                        );

                    user.FullName =
                        ReadString(
                            dataReader,
                            "FullName"
                        );

                    user.Email =
                        ReadString(
                            dataReader,
                            "Email"
                        );

                    user.Password =
                        string.Empty;

                    user.PasswordHash =
                        ReadString(
                            dataReader,
                            "PasswordHash"
                        );

                    user.Role =
                        ReadString(
                            dataReader,
                            "Role"
                        );

                    user.IsLocked =
                        Convert.ToBoolean(
                            dataReader["IsLocked"]
                        );

                    user.CanShare =
                        Convert.ToBoolean(
                            dataReader["CanShare"]
                        );

                    user.CreatedAt =
                        Convert.ToDateTime(
                            dataReader["CreatedAt"]
                        );

                    user.UpdatedAt =
                        ReadNullableDateTime(
                            dataReader,
                            "UpdatedAt"
                        );

                    user.LastLoginAt =
                        ReadNullableDateTime(
                            dataReader,
                            "LastLoginAt"
                        );

                    return user;
                }

                return null;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 3. Get user by ID
        // SP_Users_GetById
        //
        // PasswordHash is not returned by this SP.
        // =====================================================

        public User? GetUserById(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_GetById",
                con,
                paramDic
            );

            try
            {
                using SqlDataReader dataReader =
                    cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(
                            dataReader["UserId"]
                        );

                    user.FullName =
                        ReadString(
                            dataReader,
                            "FullName"
                        );

                    user.Email =
                        ReadString(
                            dataReader,
                            "Email"
                        );

                    /*
                     * SP_Users_GetById does not return
                     * PasswordHash.
                     */
                    user.Password =
                        string.Empty;

                    user.PasswordHash =
                        string.Empty;

                    user.Role =
                        ReadString(
                            dataReader,
                            "Role"
                        );

                    user.IsLocked =
                        Convert.ToBoolean(
                            dataReader["IsLocked"]
                        );

                    user.CanShare =
                        Convert.ToBoolean(
                            dataReader["CanShare"]
                        );

                    user.CreatedAt =
                        Convert.ToDateTime(
                            dataReader["CreatedAt"]
                        );

                    user.UpdatedAt =
                        ReadNullableDateTime(
                            dataReader,
                            "UpdatedAt"
                        );

                    user.LastLoginAt =
                        ReadNullableDateTime(
                            dataReader,
                            "LastLoginAt"
                        );

                    return user;
                }

                return null;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 4. Update user profile
        // SP_Users_UpdateProfile
        //
        // Returns:
        // 1  = updated
        // 0  = user not found
        // -1 = duplicate email
        // =====================================================

        public int UpdateUserProfile(User user)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", user.UserId);
            paramDic.Add("@FullName", user.FullName);
            paramDic.Add("@Email", user.Email);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_UpdateProfile",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 5. Update user password
        // SP_Users_UpdatePassword
        //
        // Returns true when one row was updated.
        // =====================================================

        public bool UpdateUserPassword(
            int userId,
            string newPasswordHash)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@NewPasswordHash",
                newPasswordHash
            );

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_UpdatePassword",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 6. Record successful login
        // SP_Users_LogLogin
        //
        // Returns true when the login was recorded.
        // =====================================================

        public bool LogUserLogin(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_Users_LogLogin",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 7. Get user continents
        // SP_UserContinents_GetByUser
        // =====================================================

        public List<string> GetUserContinents(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            List<string> continents =
                new List<string>();

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserContinents_GetByUser",
                con,
                paramDic
            );

            try
            {
                using SqlDataReader dataReader =
                    cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string continentName =
                        ReadString(
                            dataReader,
                            "ContinentName"
                        );

                    continents.Add(continentName);
                }

                return continents;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 8. Add continent preference
        // SP_UserContinents_Add
        //
        // Returns:
        // positive number = new UserContinentId
        // -1              = duplicate
        // 0               = no result
        // =====================================================

        public int AddUserContinent(
            int userId,
            string continentName)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@ContinentName",
                continentName
            );

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserContinents_Add",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 9. Remove continent preference
        // SP_UserContinents_Remove
        // =====================================================

        public bool RemoveUserContinent(
            int userId,
            string continentName)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@ContinentName",
                continentName
            );

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserContinents_Remove",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 10. Get user languages
        // SP_UserLanguages_GetByUser
        // =====================================================

        public List<UserLanguage> GetUserLanguages(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            List<UserLanguage> languages =
                new List<UserLanguage>();

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserLanguages_GetByUser",
                con,
                paramDic
            );

            try
            {
                using SqlDataReader dataReader =
                    cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    UserLanguage language =
                        new UserLanguage();

                    language.LanguageCode =
                        ReadString(
                            dataReader,
                            "LanguageCode"
                        );

                    language.LanguageName =
                        ReadString(
                            dataReader,
                            "LanguageName"
                        );

                    language.ProficiencyLevel =
                        ReadString(
                            dataReader,
                            "ProficiencyLevel"
                        );

                    languages.Add(language);
                }

                return languages;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 11. Add user language
        // SP_UserLanguages_Add
        //
        // Returns:
        // positive number = new UserLanguageId
        // -1              = duplicate
        // 0               = no result
        // =====================================================

        public int AddUserLanguage(
            int userId,
            UserLanguage language)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@LanguageCode",
                language.LanguageCode
            );

            paramDic.Add(
                "@LanguageName",
                language.LanguageName
            );

            paramDic.Add(
                "@ProficiencyLevel",
                language.ProficiencyLevel
            );

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserLanguages_Add",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 12. Update user language
        // SP_UserLanguages_Update
        // =====================================================

        public bool UpdateUserLanguage(
            int userId,
            string languageCode,
            string proficiencyLevel)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@LanguageCode",
                languageCode
            );

            paramDic.Add(
                "@ProficiencyLevel",
                proficiencyLevel
            );

            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_UserLanguages_Update",
                con,
                paramDic
            );

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // 13. Remove user language
        // SP_UserLanguages_Remove
        // =====================================================

        public bool RemoveUserLanguage(
            int userId,
            string languageCode)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch
            {
                throw;
            }

            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            paramDic.Add("@UserId", userId);

            paramDic.Add(
                "@LanguageCode",languageCode);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UserLanguages_Remove",con, paramDic);

            try
            {
                var result = cmd.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // =====================================================
        // Create command with stored procedure
        // =====================================================

        private SqlCommand CreateCommandWithStoredProcedureGeneral(
            string spName,
            SqlConnection con,
            Dictionary<string, object> paramDic)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (
                KeyValuePair<string, object> param
                in paramDic)
            {
                cmd.Parameters.AddWithValue(
                    param.Key,
                    param.Value
                );
            }

            return cmd;
        }

        // =====================================================
        // Reader helper methods
        // =====================================================

        private string ReadString(
            SqlDataReader dataReader,
            string columnName)
        {
            if (dataReader[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return (string)dataReader[columnName];
        }

        private DateTime ReadNullableDateTime(
            SqlDataReader dataReader,
            string columnName)
        {
            if (dataReader[columnName] == DBNull.Value)
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(
                dataReader[columnName]
            );
        }
    }
}
