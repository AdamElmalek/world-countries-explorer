using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
        public DBservices()
        {
        }

        public SqlConnection connect(string conString)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: false
                    )
                    .Build();

            string? connectionString =
                configuration.GetConnectionString(conString);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception(
                    "Connection string '" +
                    conString +
                    "' was not found in appsettings.json."
                );
            }

            SqlConnection con =
                new SqlConnection(connectionString);

            con.Open();

            return con;
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
    }
}
