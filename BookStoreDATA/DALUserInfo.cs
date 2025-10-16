using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDATA
{
    public class DALUserInfo
    {
        public int LogIn(string userName, string password)
        {
            var conn = new SqlConnection(Properties.Settings.Default.cpConnection);

            // sql stuff
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // The Query
                cmd.CommandText =
                "SELECT UserID " +
                "FROM UserData " +
                "WHERE UserName = @UserName AND Password = @PassWord ";

                // Query Parameters
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@PassWord", password);
                conn.Open();

                // Execute the query and check if null
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int UserId = (int)result;
                    if (UserId > 0)
                        return UserId;
                    else
                        return -1;
                }
                else { return -1; } // No user in the database
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
        }
    }
}
