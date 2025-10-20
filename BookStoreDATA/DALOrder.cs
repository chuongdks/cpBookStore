using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDATA
{
    public class DALOrder {
        public int Proceed2Order(string xmlOrder)
        {
            SqlConnection cn = new SqlConnection(Properties.Settings.Default.cpConnection);

            // SQL stuff
            try
            {
                // create a Store Procedure name "insertOrder"
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insertOrder";

                // Input Parameter for the XML Data
                SqlParameter inParameter = new SqlParameter();
                inParameter.ParameterName = "@xml";
                inParameter.Value = xmlOrder;
                inParameter.DbType = DbType.String;
                inParameter.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(inParameter);

                // Return Parameter
                SqlParameter ReturnParameter = new SqlParameter();
                ReturnParameter.ParameterName = "@OrderID";
                ReturnParameter.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(ReturnParameter);

                // execute the query
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                return (int)cmd.Parameters["@OrderID"].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (cn.State == ConnectionState.Open) {
                    cn.Close();
                }
            }
        }
    }
}
