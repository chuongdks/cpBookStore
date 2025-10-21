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
    public class DALBookCatalog {
        SqlConnection conn;
        DataSet dsBooks;

        public DALBookCatalog() {
            conn = new SqlConnection(Properties.Settings.Default.cpConnection);
        }

        // Method to get the book info and catalog when MainWindow is loaded
        public DataSet GetBookInfo() {
            // sql stuff
            try
            {
                dsBooks = new DataSet("Books");
                // Query for Category Table
                String strSQL =
                "SELECT CategoryID, Name, Description " +
                "FROM Category";

                // Assign Query and Connection
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);                
                daCategory.Fill(dsBooks, "Category");   // Get category info

                // Query for BookData Table 
                String strSQL2 =
                "SELECT ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher " +
                "FROM BookData";

                // Assign Query and Connection
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "BookData");          // Get Books info

                // Data Relation
                DataColumn parentColumn = dsBooks.Tables["Category"].Columns["CategoryID"];
                DataColumn childColumn = dsBooks.Tables["BookData"].Columns["CategoryID"];

                DataRelation drCat_Book = new DataRelation("drCat_Book", parentColumn, childColumn, false);

                dsBooks.Relations.Add(drCat_Book);      // Set up the table relation
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dsBooks;
        }

        // Method to call the 'insertBook' stored procedure
        public bool InsertBook(string xmlBook)
        {
            try
            {
                // create a Store Procedure name "insertBook"
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insertBook";

                // Input Parameter for the XML Data
                SqlParameter inParameter = new SqlParameter();
                inParameter.ParameterName = "@xml"; 
                inParameter.Value = xmlBook;
                inParameter.DbType = DbType.String;
                inParameter.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(inParameter);

                // execute the query
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();   // should return 1 affected row for the book insert
                conn.Close();

                // return bool check 
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                // CRUCIAL: Catches database-specific errors (e.g., duplicate ISBN)
                Debug.WriteLine("SQL Error during book insertion: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
    }
}
