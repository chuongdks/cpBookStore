using BookStoreLIB;
using System;
using System.Collections.Generic;
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

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for RegisterDialog.xaml
    /// </summary>
    public partial class RegisterDialog : Window
    {
        public RegisterDialog()
        {
            InitializeComponent();
        }

        // put the method here and not MainWindow cuz the register dialog needs to check first
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = userNameTextBox.Text;
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;
            string fullName = fullNameTextBox.Text;

            // a confirm password logic
            if (password != confirmPassword)
            {
                MessageBox.Show("Password and Confirm Password must match.", "Input Error");
                confirmPasswordBox.Clear();
                return;
            }

            UserData userData = new UserData();

            // Call the LIB layer 
            string result = userData.RegisterUser(userName, password, fullName);

            if (result == null)
            {
                // Success register
                MessageBox.Show("Registration successful! You can now log in.", "Success");
                this.DialogResult = true;
            }
            else
            {
                // Failed register
                MessageBox.Show(result, "Registration Failed");
            }
        }
    }
}
