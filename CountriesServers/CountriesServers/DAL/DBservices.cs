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

        // 3. Get user by ID
        // SP_Users_GetById

        // PasswordHash is not returned by this SP.

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

        // 4. Update user profile
        // SP_Users_UpdateProfile

        // Returns:
        // 1  = updated
        // 0  = user not found
        // -1 = duplicate email

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

        // 5. Update user password
        // SP_Users_UpdatePassword


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

        // 6. Record successful login
        // SP_Users_LogLogin
        //
        // Returns true when the login was recorded.


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


        // 9. Remove continent preference
        // SP_UserContinents_Remove


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

        // 10. Get user languages
        // SP_UserLanguages_GetByUser

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

        // 11. Add user language
        // SP_UserLanguages_Add
        //
        // Returns:
        // positive number = new UserLanguageId
        // -1              = duplicate
        // 0               = no result

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

        // 12. Update user language
        // SP_UserLanguages_Update

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

        // 13. Remove user language
        // SP_UserLanguages_Remove

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
                "@LanguageCode", languageCode);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UserLanguages_Remove", con, paramDic);

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

        // Create command with stored procedure

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

        // Reader helper methods


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
        private object GetDatabaseValue(string value)
        {
            if (value == string.Empty)
            {
                return DBNull.Value;
            }

            return value;
        }
        //helper create country params
        private Dictionary<string, object> CreateCountryParameters(Country country, bool includeCountryId)
        {
            Dictionary<string, object> paramDic =
                new Dictionary<string, object>();

            if (includeCountryId)
            {
                paramDic.Add("@CountryId", country.CountryId);
            }

            paramDic.Add("@Name", country.Name);
            paramDic.Add("@NativeName", GetDatabaseValue(country.NativeName));
            paramDic.Add("@Alpha2Code", country.Alpha2Code);
            paramDic.Add("@Alpha3Code", country.Alpha3Code);
            paramDic.Add("@NumericCode", GetDatabaseValue(country.NumericCode));
            paramDic.Add("@Capital", GetDatabaseValue(country.Capital));
            paramDic.Add("@Region", GetDatabaseValue(country.Region));
            paramDic.Add("@Subregion", GetDatabaseValue(country.Subregion));
            paramDic.Add("@Population", country.Population);
            paramDic.Add("@Area", country.Area);
            paramDic.Add("@Latitude", country.Lat);
            paramDic.Add("@Longitude", country.Lng);
            paramDic.Add("@FlagUrl", GetDatabaseValue(country.FlagUrl));
            paramDic.Add("@FlagPngUrl", GetDatabaseValue(country.FlagPngUrl));
            paramDic.Add("@FlagEmoji", GetDatabaseValue(country.FlagEmoji));
            paramDic.Add("@Demonym", GetDatabaseValue(country.Demonym));
            paramDic.Add("@PopulationDensity", country.PopulationDensity);

            return paramDic;
        }
        //country imporrt method
        public int UpsertCountry(Country country)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                    CreateCountryParameters(country, false);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_Upsert",
                    con,
                    paramDic
                );

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //country insert
        public int InsertCountry(Country country)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                    CreateCountryParameters(country, false);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_Insert",
                    con,
                    paramDic
                );

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //country update 1= updated, 0=invalid or notfund , -1= dupelicated
        public int UpdateCountry(Country country)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                    CreateCountryParameters(country, true);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_Update",
                    con,
                    paramDic
                );

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
        //delete country
        public bool DeleteCountry(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                   new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_Delete",
                    con,
                    paramDic
                );

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
        //  country lang methods 
        public int DeleteCountryLanguages(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                   new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryLanguages_DeleteByCountry",
                    con,
                    paramDic
                );


                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public int AddCountryLanguage(int countryId, CountryLanguage language)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                   new Dictionary<string, object>();

                paramDic.Add("@CountryId", countryId);
                paramDic.Add("@LanguageCode", language.LanguageCode);
                paramDic.Add("@LanguageName", language.LanguageName);
                paramDic.Add("@NativeName", GetDatabaseValue(language.NativeName));

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryLanguages_Add",
                    con,
                    paramDic
                );

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //Countr currencies methods
        public int DeleteCountryCurrencies(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                   new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryCurrencies_DeleteByCountry",
                    con,
                    paramDic
                );


                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public int AddCountryCurrency(int countryId, CountryCurrency currency)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic =
                   new Dictionary<string, object>();

                paramDic.Add("@CountryId", countryId);
                paramDic.Add("@currencyCode", currency.CurrencyCode);
                paramDic.Add("@currencyName", currency.CurrencyName);
                paramDic.Add("@CurrencySymbol", GetDatabaseValue(currency.CurrencySymbol));

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryCurrencies_Add",
                    con,
                    paramDic
                );

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //Numeric reader helpers
        private long ReadLong(SqlDataReader dataReader, string columnName)
        {
            if (dataReader[columnName] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt64(dataReader[columnName]);
        }

        private decimal ReadDecimal(SqlDataReader dataReader, string columnName)
        {
            if (dataReader[columnName] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToDecimal(dataReader[columnName]);
        }

        //get country languages
        public List<CountryLanguage> GetCountryLanguages(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<CountryLanguage> languages = new List<CountryLanguage>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryLanguages_GetByCountry",
                    con,
                    paramDic
                );

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    CountryLanguage language = new CountryLanguage();

                    language.CountryLanguageId = Convert.ToInt32(dataReader["CountryLanguageId"]);
                    language.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                    language.LanguageCode = ReadString(dataReader, "LanguageCode");
                    language.LanguageName = ReadString(dataReader, "LanguageName");
                    language.NativeName = ReadString(dataReader, "NativeName");

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
                cmd.Dispose();
                con.Close();
            }
        }

        //get country currencues
        public List<CountryCurrency> GetCountryCurrencies(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<CountryCurrency> currencies = new List<CountryCurrency>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_CountryCurrencies_GetByCountry",
                    con,
                    paramDic
                );

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    CountryCurrency currency = new CountryCurrency();

                    currency.CountryCurrencyId = Convert.ToInt32(dataReader["CountryCurrencyId"]);
                    currency.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                    currency.CurrencyCode = ReadString(dataReader, "CurrencyCode");
                    currency.CurrencyName = ReadString(dataReader, "CurrencyName");
                    currency.CurrencySymbol = ReadString(dataReader, "CurrencySymbol");

                    currencies.Add(currency);
                }

                return currencies;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //converting a details row into countries obj
        private Country ReadCountryDetails(SqlDataReader dataReader)
        {
            Country country = new Country();

            country.CountryId = Convert.ToInt32(dataReader["CountryId"]);
            country.Name = ReadString(dataReader, "Name");
            country.NativeName = ReadString(dataReader, "NativeName");
            country.Alpha2Code = ReadString(dataReader, "Alpha2Code");
            country.Alpha3Code = ReadString(dataReader, "Alpha3Code");
            country.NumericCode = ReadString(dataReader, "NumericCode");
            country.Capital = ReadString(dataReader, "Capital");
            country.Region = ReadString(dataReader, "Region");
            country.Subregion = ReadString(dataReader, "Subregion");
            country.Population = ReadLong(dataReader, "Population");
            country.Area = ReadDecimal(dataReader, "Area");
            country.Lat = ReadDecimal(dataReader, "Latitude");
            country.Lng = ReadDecimal(dataReader, "Longitude");
            country.FlagUrl = ReadString(dataReader, "FlagUrl");
            country.FlagPngUrl = ReadString(dataReader, "FlagPngUrl");
            country.FlagEmoji = ReadString(dataReader, "FlagEmoji");
            country.Demonym = ReadString(dataReader, "Demonym");
            country.PopulationDensity = ReadDecimal(dataReader, "PopulationDensity");
            country.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
            country.UpdatedAt = ReadNullableDateTime(dataReader, "UpdatedAt");

            return country;
        }

        public Country? GetCountryById(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            Country? country = null;

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_GetById",
                    con,
                    paramDic
                );

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        country = ReadCountryDetails(dataReader);
                    }
                }

                con.Close();

                if (country != null)
                {
                    country.LanguageList = GetCountryLanguages(countryId);
                    country.CurrencyList = GetCountryCurrencies(countryId);
                }

                return country;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //convert a search riw into a countr object
        private Country ReadCountrySearchRow(SqlDataReader dataReader)
        {
            Country country = new Country();

            country.CountryId = Convert.ToInt32(dataReader["CountryId"]);
            country.Name = ReadString(dataReader, "Name");
            country.NativeName = ReadString(dataReader, "NativeName");
            country.Alpha2Code = ReadString(dataReader, "Alpha2Code");
            country.Alpha3Code = ReadString(dataReader, "Alpha3Code");
            country.Capital = ReadString(dataReader, "Capital");
            country.Region = ReadString(dataReader, "Region");
            country.Subregion = ReadString(dataReader, "Subregion");
            country.Population = ReadLong(dataReader, "Population");
            country.Area = ReadDecimal(dataReader, "Area");
            country.Lat = ReadDecimal(dataReader, "Latitude");
            country.Lng = ReadDecimal(dataReader, "Longitude");
            country.FlagUrl = ReadString(dataReader, "FlagUrl");
            country.FlagPngUrl = ReadString(dataReader, "FlagPngUrl");
            country.FlagEmoji = ReadString(dataReader, "FlagEmoji");
            country.Demonym = ReadString(dataReader, "Demonym");
            country.PopulationDensity = ReadDecimal(dataReader, "PopulationDensity");
            country.Languages = ReadString(dataReader, "Languages");
            country.Currencies = ReadString(dataReader, "Currencies");

            return country;
        }

        //Search, filter and sort countries
        public List<Country> SearchCountries(string search, string region, string language, string currency, long? minPopulation, long? maxPopulation, decimal? minArea, decimal? maxArea, string sortBy, string direction)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<Country> countries = new List<Country>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                paramDic.Add("@Search", GetDatabaseValue(search));
                paramDic.Add("@Region", GetDatabaseValue(region));
                paramDic.Add("@Language", GetDatabaseValue(language));
                paramDic.Add("@Currency", GetDatabaseValue(currency));

                if (minPopulation.HasValue)
                {
                    paramDic.Add("@MinPopulation", minPopulation.Value);
                }
                else
                {
                    paramDic.Add("@MinPopulation", DBNull.Value);
                }

                if (maxPopulation.HasValue)
                {
                    paramDic.Add("@MaxPopulation", maxPopulation.Value);
                }
                else
                {
                    paramDic.Add("@MaxPopulation", DBNull.Value);
                }

                if (minArea.HasValue)
                {
                    paramDic.Add("@MinArea", minArea.Value);
                }
                else
                {
                    paramDic.Add("@MinArea", DBNull.Value);
                }

                if (maxArea.HasValue)
                {
                    paramDic.Add("@MaxArea", maxArea.Value);
                }
                else
                {
                    paramDic.Add("@MaxArea", DBNull.Value);
                }

                paramDic.Add("@SortBy", sortBy);
                paramDic.Add("@Direction", direction);

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_Search",
                    con,
                    paramDic
                );

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    countries.Add(ReadCountrySearchRow(dataReader));
                }

                return countries;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //get filter options
        public CountryFilterOptions GetCountryFilterOptions()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            CountryFilterOptions options = new CountryFilterOptions();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                cmd = CreateCommandWithStoredProcedureGeneral(
                    "SP_Countries_GetFilterOptions",
                    con,
                    paramDic
                );

                using SqlDataReader dataReader = cmd.ExecuteReader();

                //result 1: regions
                while (dataReader.Read())
                {
                    options.Regions.Add(ReadString(dataReader, "Region"));
                }

                //result 2: languages
                dataReader.NextResult();

                while (dataReader.Read())
                {
                    CountryLanguage language = new CountryLanguage();

                    language.LanguageCode = ReadString(dataReader, "LanguageCode");
                    language.LanguageName = ReadString(dataReader, "LanguageName");

                    options.Languages.Add(language);
                }

                //result 3: currencies
                dataReader.NextResult();

                while (dataReader.Read())
                {
                    CountryCurrency currency = new CountryCurrency();

                    currency.CurrencyCode = ReadString(dataReader, "CurrencyCode");
                    currency.CurrencyName = ReadString(dataReader, "CurrencyName");

                    options.Currencies.Add(currency);
                }

                //result 4: population and area limits
                dataReader.NextResult();

                if (dataReader.Read())
                {
                    options.MinPopulation = ReadLong(dataReader, "MinPopulation");
                    options.MaxPopulation = ReadLong(dataReader, "MaxPopulation");
                    options.MinArea = ReadDecimal(dataReader, "MinArea");
                    options.MaxArea = ReadDecimal(dataReader, "MaxArea");
                }

                return options;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public int AddCountryToList(int userId, int countryId, string listType)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();


                paramDic.Add("@CountryId", countryId);
                paramDic.Add("@UserId", userId);
                paramDic.Add("@ListType", listType);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserCountryLists_Add", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());


            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public List<UserCountryList> GetCountriesByListType(int userId, string listType)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            List<UserCountryList> userCountries = new List<UserCountryList>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@ListType", listType);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserCountryLists_GetByType", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    UserCountryList userCountry = new UserCountryList();

                    userCountry.UserCountryListId = Convert.ToInt32(dataReader["UserCountryListId"]);
                    userCountry.UserId = Convert.ToInt32(dataReader["UserId"]);
                    userCountry.CountryId = Convert.ToInt32(dataReader["CountryId"]);

                    userCountry.ListType = ReadString(dataReader, "ListType");
                    userCountry.SavedAt = Convert.ToDateTime(dataReader["SavedAt"]);

                    Country country = new Country();

                    country.CountryId = userCountry.CountryId;
                    country.Name = ReadString(dataReader, "Name");
                    country.NativeName = ReadString(dataReader, "NativeName");
                    country.Alpha2Code = ReadString(dataReader, "Alpha2Code");
                    country.Alpha3Code = ReadString(dataReader, "Alpha3Code");
                    country.Capital = ReadString(dataReader, "Capital");
                    country.Region = ReadString(dataReader, "Region");
                    country.Subregion = ReadString(dataReader, "Subregion");

                    if (dataReader["Population"] != DBNull.Value)
                    {
                        country.Population = Convert.ToInt64(dataReader["Population"]);

                    }

                    if (dataReader["Area"] != DBNull.Value)
                    {
                        country.Area = Convert.ToDecimal(dataReader["Area"]);

                    }

                    country.FlagUrl = ReadString(dataReader, "FlagUrl");


                    country.FlagPngUrl = ReadString(dataReader, "FlagPngUrl");


                    country.FlagEmoji = ReadString(dataReader, "FlagEmoji");


                    userCountry.Country = country;

                    userCountries.Add(userCountry);
                }
                return userCountries;

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public bool MoveCountryToList(int userId, int countryId, string toType)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();


                paramDic.Add("@UserId", userId);
                paramDic.Add("@CountryId", countryId);
                paramDic.Add("@ToType", toType);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserCountryLists_Move", con, paramDic);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result == 1;

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public bool RemoveCountryFromList(int userId, int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();


                paramDic.Add("@UserId", userId);
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserCountryLists_Remove", con, paramDic);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result == 1;

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }


        public UserCountryList? GetCountryListStatus(int userId, int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserCountryLists_GetCountryStatus", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    UserCountryList userCountry = new UserCountryList();

                    userCountry.UserCountryListId = Convert.ToInt32(dataReader["UserCountryListId"]);
                    userCountry.UserId = Convert.ToInt32(dataReader["UserId"]);
                    userCountry.CountryId = Convert.ToInt32(dataReader["CountryId"]);

                    userCountry.ListType = ReadString(dataReader, "ListType");
                    userCountry.SavedAt = Convert.ToDateTime(dataReader["SavedAt"]);

                    return userCountry;

                }
                return null;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public int AddShare(Share share)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", share.UserId);
                paramDic.Add("@CountryId", share.CountryId);
                paramDic.Add("@Content", share.Content);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Shares_Add", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public List<Share> GetAllShares()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<Share> shares = new List<Share>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Shares_GetAll", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    shares.Add(ReadShare(dataReader));
                }

                return shares;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public List<Share> GetSharesByCountry(int countryId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<Share> shares = new List<Share>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@CountryId", countryId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Shares_GetByCountry", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    shares.Add(ReadShare(dataReader));
                }

                return shares;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }


        public int UpdateShare(Share share)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@ShareId", share.ShareId);
                paramDic.Add("@UserId", share.UserId);
                paramDic.Add("@Content", share.Content);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Shares_Update", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }


        public bool DeleteShare(int shareId, int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@ShareId", shareId);
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Shares_Delete", con, paramDic);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result == 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }


        private Share ReadShare(SqlDataReader dataReader)
        {
            Share share = new Share();

            share.ShareId = Convert.ToInt32(dataReader["ShareId"]);
            share.UserId = Convert.ToInt32(dataReader["UserId"]);
            share.CountryId = Convert.ToInt32(dataReader["CountryId"]);

            share.Content = ReadString(dataReader, "Content");
            share.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
            share.UpdatedAt = ReadNullableDateTime(dataReader, "UpdatedAt");

            share.UserName = ReadString(dataReader, "UserName");
            share.CountryName = ReadString(dataReader, "CountryName");
            share.Alpha3Code = ReadString(dataReader, "Alpha3Code");
            share.FlagUrl = ReadString(dataReader, "FlagUrl");
            share.FlagPngUrl = ReadString(dataReader, "FlagPngUrl");

            return share;
        }

        //SP_Quizzes_GetActive
        public List<Quiz> GetActiveQuizzes()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<Quiz> quizzes = new List<Quiz>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Quizzes_GetActive", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Quiz quiz = new Quiz();

                    quiz.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    quiz.Title = ReadString(dataReader, "Title");
                    quiz.Description = ReadString(dataReader, "Description");
                    quiz.TimeLimitSeconds = Convert.ToInt32(dataReader["TimeLimitSeconds"]);
                    quiz.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    quiz.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                    quiz.QuestionsCount = Convert.ToInt32(dataReader["QuestionsCount"]);
                    quiz.MaximumScore = Convert.ToInt32(dataReader["MaximumScore"]);

                    quizzes.Add(quiz);

                }

                return quizzes;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //SP_QuizQuestions_GetByQuiz
        public List<QuizQuestion> GetQuizQuestions(int quizId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<QuizQuestion> questions = new List<QuizQuestion>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@QuizId", quizId);
                cmd = CreateCommandWithStoredProcedureGeneral("SP_QuizQuestions_GetByQuiz", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    QuizQuestion question = new QuizQuestion();

                    question.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
                    question.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    question.QuestionText = ReadString(dataReader, "QuestionText");
                    question.OptionA = ReadString(dataReader, "OptionA");
                    question.OptionB = ReadString(dataReader, "OptionB");
                    question.OptionC = ReadString(dataReader, "OptionC");
                    question.OptionD = ReadString(dataReader, "OptionD");
                    question.Points = Convert.ToInt32(dataReader["Points"]);

                    questions.Add(question);

                }

                return questions;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get correct answers for server-side score calculation :SP_QuizQuestions_GetCorrectAnswers
        public List<QuizCorrectAnswer> GetQuizCorrectAnswers(int quizId)
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<QuizCorrectAnswer> answers = new List<QuizCorrectAnswer>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@QuizId", quizId);
                cmd = CreateCommandWithStoredProcedureGeneral("SP_QuizQuestions_GetCorrectAnswers", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    QuizCorrectAnswer answer = new QuizCorrectAnswer();

                    answer.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
                    answer.CorrectOption = ReadString(dataReader, "CorrectOption");
                    answer.Points = Convert.ToInt32(dataReader["Points"]);

                    answers.Add(answer);

                }

                return answers;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Start a new quiz attempt. :SP_UserQuizAttempts_Start
        public int StartQuizAttempt(int userId, int quizId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@QuizId", quizId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_Start", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Save the completed quiz result. :SP_UserQuizAttempts_Complete
        public int CompleteQuizAttempt(int attemptId, int userId, int quizId, int score, int correctAnswers, int totalQuestions)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@AttemptId", attemptId);
                paramDic.Add("@UserId", userId);
                paramDic.Add("@QuizId", quizId);
                paramDic.Add("@Score", score);
                paramDic.Add("@CorrectAnswers", correctAnswers);
                paramDic.Add("@TotalQuestions", totalQuestions);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_Complete", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get all quiz attempts made by one user.:SP_UserQuizAttempts_GetByUser
        public List<UserQuizAttempt> GetQuizAttemptsByUser(int userId)
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<UserQuizAttempt> attempts = new List<UserQuizAttempt>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_GetByUser", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    UserQuizAttempt attempt = new UserQuizAttempt();

                    attempt.AttemptId = Convert.ToInt32(dataReader["AttemptId"]);
                    attempt.UserId = Convert.ToInt32(dataReader["UserId"]);
                    attempt.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    attempt.Title = ReadString(dataReader, "Title");
                    attempt.Score = Convert.ToInt32(dataReader["Score"]);
                    attempt.CorrectAnswers = Convert.ToInt32(dataReader["CorrectAnswers"]);
                    attempt.TotalQuestions = Convert.ToInt32(dataReader["TotalQuestions"]);
                    attempt.StartedAt = Convert.ToDateTime(dataReader["StartedAt"]);
                    attempt.FinishedAt = ReadNullableDateTime(dataReader, "FinishedAt");
                    attempt.IsCompleted = Convert.ToBoolean(dataReader["IsCompleted"]);

                    attempts.Add(attempt);

                }

                return attempts;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get the sum of the user's best score in every quiz.: SP_UserQuizAttempts_GetTotalPoints
        public int GetTotalQuizPoints(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_GetTotalPoints", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }


    }
}
