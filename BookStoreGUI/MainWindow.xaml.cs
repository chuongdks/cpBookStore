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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Login Dialog Box
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();

            // 
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

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"].DefaultView;

            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
        }

        private void addButton_Click(object sender, RoutedEventArgs e) 
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;

            // Get the currently selected row from the data grid
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItem;

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

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }


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
