using BookStoreDATA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class BookCatalog
    {
        // get book info and catalog when main program is loaded
        public DataSet GetBookInfo()
        {
            // perform any business logic before passing to client
            DALBookCatalog bookCatalog = new DALBookCatalog();
            return bookCatalog.GetBookInfo();
        }

        // add a new book
        public bool AddNewBook(
            string isbn, int categoryId, string title, string author, double price,
            int supplierId, string year, string edition, string publisher, int inStock)   
        { 
            // Business Logic stuff (string not empty, eg)
            if (string.IsNullOrWhiteSpace(isbn) || string.IsNullOrWhiteSpace(title) || price <= 0)
            {
                Debug.WriteLine("Validation failed: One of the field is empty");
                return false;
            }

            // XML Construction
            string xmlBook = $@"
            <Book
                ISBN='{isbn}'
                CategoryID='{categoryId}'
                Title='{title}'
                Author='{author}'
                Price='{price}'
                SupplierId='{supplierId}'
                Year='{year}'
                Edition='{edition}'
                Publisher='{publisher}'
                InStock='{inStock}'
            />";

            // DAL stuff
            try
            {
                DALBookCatalog bookCatalog = new DALBookCatalog();
                // Delegate the database insertion to the DAL
                return bookCatalog.InsertBook(xmlBook);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LIB Error: Failed to add new book. " + ex.Message);
                return false;
            }
        }
    }
}
