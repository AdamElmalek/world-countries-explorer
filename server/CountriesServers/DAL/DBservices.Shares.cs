using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
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
    }
}
