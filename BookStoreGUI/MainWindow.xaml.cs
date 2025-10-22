using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;
        BookCatalog bookCatalog;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Helper method to Refresh the GUI
        private void UpdateBookCatalogView()
        {
            // Call the Business Logic Layer to get the fresh data
            dsBookCat = bookCatalog.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"].DefaultView;

            // Optional: Reset the selected item in the ComboBox to the first item
            if (this.categoriesComboBox.Items.Count > 0)
            {
                this.categoriesComboBox.SelectedIndex = 0;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bookCatalog = new BookCatalog();
            bookOrder = new BookOrder();
            userData = new UserData();

            dsBookCat = bookCatalog.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"].DefaultView;    // Set the DataContext part to use dsBookCat's "Category" Table    

            this.orderListView.ItemsSource = bookOrder.OrderItemList;       // Set the ItemSource to use ObservableCollection of "orderItemList"
        }
        
        //
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Login Dialog Box
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();

            // Fill login info based on Login Dialog
            string username = dlg.nameTextBox.Text;
            string password = dlg.passwordTextBox.Password;

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                string validationMessage = userData.ValidateLogIn(username, password);

                // Log In info is invalid
                if (validationMessage != null)
                {
                    MessageBox.Show(validationMessage);
                    return;
                }

                // Attempt to Log In and check database
                if (userData.LogIn(username, password) == true)
                {
                    MessageBox.Show("You are logged in as User #" + userData.UserID);
                    this.statusTextBlock.Text = "Welcome back " + userData.UserName;
                }
                else
                {
                    MessageBox.Show("You could not be verified. Please try again.");
                }
            }
        }
        
        //
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        //
        private void addButton_Click(object sender, RoutedEventArgs e) 
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;

            // Get the currently selected row from the data grid
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItem;  // choose info row from the ProductsDataGrid

            // Check if a row was actually selected
            if (selectedRow == null)
            {
                MessageBox.Show("Please select a book from the catalog first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Assign specific column values to the TextBoxes
            orderItemDialog.isbnTextBox.Text = selectedRow["ISBN"].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow["Title"].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow["Price"].ToString();

            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();   

            if (orderItemDialog.DialogResult == true) {
                string isbn = orderItemDialog.isbnTextBox.Text; 
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);         // null value when add book with null quantity         
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
            }
        }
        
        //
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }
        
        //
        private void insertBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Create and show the data entry form
            AddBookDialog bookdlg = new AddBookDialog();
            bookdlg.Owner = this;
            bookdlg.ShowDialog();

            // ShowDialog() blocks the parent window until the dialog is closed.
            if (bookdlg.DialogResult == true)
            {
                try
                {
                    // 2. Retrieve validated data from the dialog
                    string isbn = bookdlg.isbnTextBox.Text;                         // Primary Key
                    int categoryId = int.Parse(bookdlg.categoryTextBox.Text);       // Foreign Key
                    string title = bookdlg.titleTextBox.Text;
                    string author = bookdlg.authorTextBox.Text;
                    double price = double.Parse(bookdlg.priceTextBox.Text);
                    int supplierId = int.Parse(bookdlg.supplierIDTextBox.Text);     // Foreign Key
                    string year = bookdlg.yearTextBox.Text;
                    string edition = bookdlg.editionTextBox.Text;
                    string publisher = bookdlg.publisherTextBox.Text;
                    int inStock = int.Parse(bookdlg.inStockTextBox.Text);

                    // 3. Call the Business Logic Layer
                    // This calls BookCatalog.AddNewBook( ... )
                    if (bookCatalog.AddNewBook(isbn, categoryId, title, author, price, supplierId, year, edition, publisher, inStock))
                    {
                        MessageBox.Show($"Book '{title}' successfully added to the catalog.", "Success");

                        // Update/Refresh GUI view
                        UpdateBookCatalogView();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add book. Check validation rules or contact IT.", "Error");
                    }
                }
                catch (FormatException)
                {
                    // Catch cases where dialog input was non-numeric for fields like Price, InStock, etc.
                    MessageBox.Show("Please ensure all numeric fields are entered correctly.", "Input Error");
                }
                catch (Exception ex)
                {
                    // Catches any unexpected system error during the process
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "System Error");
                }
            }
        }
        //
        private void chechoutButton_Click(object sender, RoutedEventArgs e) 
        {
            // 
            if (userData.UserID < 1 || bookOrder.OrderItemList.Count == 0) {
                MessageBox.Show("Please sign in and select book(s) before placing the order.");
            }
            else {
                int orderID;
                orderID = bookOrder.PlaceOrder(userData.UserID);
                MessageBox.Show("Your order has been placed. Your order id is: " + orderID.ToString());
            }
        }
    }
}
