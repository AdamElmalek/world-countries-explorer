using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
        public bool IsAdmin(int userId)

        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_IsAdmin", con, paramDic);

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

        public List<User> AdminGetUsers()
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_GetUsers", con, paramDic);

                List<User> users = new List<User>();

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    User user = new User();

                    user.UserId = Convert.ToInt32(dataReader["UserId"]);
                    user.FullName = ReadString(dataReader, "FullName");
                    user.Email = ReadString(dataReader, "Email");
                    user.Role = ReadString(dataReader, "Role");
                    user.IsLocked = Convert.ToBoolean(dataReader["IsLocked"]);
                    user.CanShare = Convert.ToBoolean(dataReader["CanShare"]);
                    user.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                    user.UpdatedAt = ReadNullableDateTime(dataReader, "UpdatedAt");
                    user.LastLoginAt = ReadNullableDateTime(dataReader, "LastLoginAt");

                    users.Add(user);
                }

                return users;
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

        public bool AdminSetUserLocked(int userId, bool isLocked)

        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@IsLocked", isLocked);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_SetUserLocked", con, paramDic);

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

        public bool AdminSetUserCanShare(int userId, bool canShare)

        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@CanShare", canShare);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_SetUserCanShare", con, paramDic);

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

        public bool AdminDeleteShare(int shareId)

        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@ShareId", shareId);


                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_DeleteShare", con, paramDic);

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
        public AdminStatistics AdminGetStatistics()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();


                cmd = CreateCommandWithStoredProcedureGeneral("SP_Admin_GetStatistics", con, paramDic);

                AdminStatistics statistics = new AdminStatistics();

                using SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    statistics.UsersCount = Convert.ToInt32(dataReader["UsersCount"]);


                    statistics.LockedUsersCount = Convert.ToInt32(dataReader["LockedUsersCount"]);


                    statistics.BlockedSharingCount = Convert.ToInt32(dataReader["BlockedSharingCount"]);


                    statistics.CountriesCount = Convert.ToInt32(dataReader["CountriesCount"]);


                    statistics.SharesCount = Convert.ToInt32(dataReader["SharesCount"]);


                    statistics.SavedCountriesCount = Convert.ToInt32(dataReader["SavedCountriesCount"]);


                    statistics.CompletedQuizAttemptsCount = Convert.ToInt32(dataReader["CompletedQuizAttemptsCount"]);

                    statistics.LoginsToday = Convert.ToInt32(dataReader["LoginsToday"]);

                }
                return statistics;
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
