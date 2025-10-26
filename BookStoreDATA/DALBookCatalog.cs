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

        // Overload 1: For initial load (no search term)
        public DataSet GetBookInfo()
        {
            return GetBookInfo(null);
        }

        // Method to get the book info and catalog when MainWindow is loaded
        public DataSet GetBookInfo(string searchTerm) {
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
                daCategory.Fill(dsBooks, "Category");   // Create new DataTable named "Category" inside the DataSet "dsBooks"

                // Query for BookData Table - WITH A SEARCH FILTER!
                String strSQL2 =
                "SELECT ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher " +
                "FROM BookData ";
                // DYNAMIC SQL FILTER LOGIC HERE
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Use LIKE and wildcards (%) to search for the term in ISBN, Title, and Author - Future: Search Price and Year with >,<,=
                    strSQL2 += "WHERE ISBN LIKE @SearchTerm " +
                               "OR Title LIKE @SearchTerm " +
                               "OR Author LIKE @SearchTerm ";
                }

                // Assign Query and Connection
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);                
                cmdSelBook.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");  // Add the searchTerm with wildcard pattern
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "BookData");       // Create new DataTable named "BookData" inside the DataSet "dsBooks"

                // Data Relation
                DataColumn parentColumn = dsBooks.Tables["Category"].Columns["CategoryID"];     // The unique identifier (the "One" side)
                DataColumn childColumn = dsBooks.Tables["BookData"].Columns["CategoryID"];      // The foreign key (the "Many" side)

                DataRelation drCat_Book = new DataRelation("drCat_Book", parentColumn, childColumn, false);     // create the Data Relation

                dsBooks.Relations.Add(drCat_Book);      // Finally add the relation in the DataSet
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dsBooks;
        }

        // Seperate searching feature but it get book by year
        public DataSet GetBooksByYear(string  yearOperator, string targetYear)
        {
            // create a new dataset
            DataSet dsSearchResults = new DataSet("SearchResults");
            string operatorSymbol;

            // switch the year operator symbol
            switch (yearOperator.ToLower())
            {
                case "before":
                    operatorSymbol = "<";
                    break;
                case "after":
                    operatorSymbol = ">";
                    break;
                case "in":
                    operatorSymbol = "=";
                    break;
                default:
                    operatorSymbol = "=";
                    break;
            }

            // sql stuff
            try
            {
                // $ icon allow variable
                String strSQL =
                "SELECT ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher " +
                "FROM BookData " +
                $"WHERE CAST(Year AS INT) {operatorSymbol} @TargetYear";

                // execute the query
                SqlCommand cmdSelBook = new SqlCommand(strSQL, conn);
                cmdSelBook.Parameters.AddWithValue("@TargetYear", targetYear);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsSearchResults, "BookData");       // Create new DataTable named "BookData" inside the DataSet "dsSearchResults"
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dsSearchResults;
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
