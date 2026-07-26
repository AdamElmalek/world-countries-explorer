using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
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
    }
}
