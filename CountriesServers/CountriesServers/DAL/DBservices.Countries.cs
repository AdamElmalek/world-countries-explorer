using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
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
    }
}
