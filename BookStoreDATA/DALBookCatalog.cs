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
    }
}
