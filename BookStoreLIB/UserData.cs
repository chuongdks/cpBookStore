using System;
using BookStoreDATA;

namespace BookStoreLIB
{
    public class UserData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // Validate username and password rules
        public string ValidateLogIn(string userName, string password)
        {
            // First check: Empty boxes
            if (userName == "" || password == "")
            {
                return "Please fill in all slots.";
            }

            // Second check: Password Length
            if (password.Length < 6)
            {
                return "Password must have at least 6 characters with characters and number";
            }

            // Third check: Must start with a capital letter
            int firstChar = password[0];
            if (!(firstChar >= 65 && firstChar <= 90))
            {
                return "Password must start with a capital letter.";
            }

            // Fourth check: Only letters and digits allowed + Must contain both letters AND digits 
            bool hasLetter = false;
            bool hasDigit = false;

            for (int i = 0; i < password.Length; i++)
            {
                int passwordAscii = password[i];

                // Digit check
                if (passwordAscii >= 48 && passwordAscii <= 57)
                {
                    hasDigit = true;
                }
                // Letter check
                else if ((passwordAscii >= 65 && passwordAscii <= 90) || (passwordAscii >= 97 && passwordAscii <= 122))
                {
                    hasLetter = true;
                }
                else
                {
                    return "Password cannot contain characters other than letters AND numbers.";
                }
            }

            if (!hasLetter || !hasDigit)
            {
                return "Password must contain both letters AND numbers.";
            }

            // Login info is valid, return null
            return null;
        }

        // Check if user info is in the db
        public bool LogIn(string userName, string password)
        {
            // Password valid, check for Correct User Name and Password
            var dbUser = new DALUserInfo();
            UserID = dbUser.LogIn(userName, password);
            if (UserID > 0)
            {
                UserName = userName;
                Password = password;
                return true;
            }
            else { return false; }
        }

        // Register new user
        public string RegisterUser(string userName, string password, string fullName)
        {
            // Validate the username and password rules
            string validationError = ValidateLogIn(userName, password);
            if (validationError != null)    // return null if both dont break teh rule
            {
                return validationError; // Return specific validation error
            }

            // DAL part
            DALUserInfo dbUser = new DALUserInfo();
            if (dbUser.InsertUser(userName, password, fullName)) {
                return null;    // Registration success
            }
            else {
                return "Registration failed. User name is already taken or other error";
            }
        }
    }
}