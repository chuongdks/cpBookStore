using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BookStoreLIB
{
    [TestClass]
    public class UnitTest1
    {
        UserData userData = new UserData();
        string inputName, inputPassword;
        int actualUserId;
        [TestMethod]
        // Test Case 1: User 1 Valid name and password
        public void TestMethod1()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "Chuong123";

            // specify value of expected outputs
            bool expectedReturn = true;
            int expectedUserId = 7;     // Row 7 user_id 

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.IsNull(validationResult);                    // Pass validation if null
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        // Test Case 2: Blank Username and Valid password
        public void TestMethod2()
        {
            // specify values of test inputs
            inputName = "";
            inputPassword = "Chuong123";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results 
            Assert.AreEqual("Please fill in all slots.", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        // Test Case 3: Invalid Username or password
        public void TestMethod3()
        {
            // specify values of test inputs
            inputName = "whoisthisguy";
            inputPassword = "Idknotonthelist123";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.IsNull(validationResult);                    // Pass validation if null
            Assert.AreEqual(expectedReturn, actualReturn);      // but not a valid user
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        // Test Case 4: Valid username, password have less than 6 chars
        public void TestMethod4()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "Ch12";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.AreEqual("Password must have at least 6 characters with characters and number", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        // Test Case 5: Valid username, password contain invalid character that are not words or digit
        public void TestMethod5()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "Chuong!2345";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.AreEqual("Password cannot contain characters other than letters AND numbers.", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        // Test Case 6: Valid username, password contain letters or numbers only, not both
        public void TestMethod6()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "Chuongpass";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.AreEqual("Password must contain both letters AND numbers.", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        // Test Case 7: Valid username, blank password
        public void TestMethod7()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "";

            // specify value of expected outputs
            bool expectedReturn = false;    // Expect to fail
            int expectedUserId = -1;        // No valid user

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.AreEqual("Please fill in all slots.", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        // Test Case 8: Valid username, password start with non capital letter
        public void TestMethod8()
        {
            // specify values of test inputs
            inputName = "pham75";
            inputPassword = "chuong123";

            // specify value of expected outputs
            bool expectedReturn = true;    // query doesnt check case sensitive but the validate function will do it
            int expectedUserId = 7;

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.AreEqual("Password must start with a capital letter.", validationResult);
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        // Test Case 9: Test other user in database
        public void TestMethod9()
        {
            // specify values of test inputs
            inputName = "cheng49";
            inputPassword = "Cheng123";

            // specify value of expected outputs
            bool expectedReturn = true;
            int expectedUserId = 9;        // Row 9 user_id 

            // obtain actual outputs by calling the method under UserData
            string validationResult = userData.ValidateLogIn(inputName, inputPassword);
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;

            // verify the results
            Assert.IsNull(validationResult);                    // Pass validation if null
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
    }
}
