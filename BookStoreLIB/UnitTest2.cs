using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreLIB
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        public UnitTest2()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddNewBook_ZeroPrice()
        {
            // Arrange: Create a BookCatalog instance
            BookCatalog catalog = new BookCatalog();

            // Act: Attempt to add a book with an invalid price (0.00)
            bool actual = catalog.AddNewBook(
                isbn: "1234567890", categoryId: 1, title: "Test Book", author: "Author Name",
                price: 0.00, supplierId: 1, year: "2025", edition: "1", publisher: "Pub Co", inStock: 5);

            // Assert: Expect the business logic validation to fail and return false
            Assert.IsFalse(actual);
        }


        [TestMethod]
        public void TestAddItem_MergeQuantity()
        {
            // Arrange: Setup order and items
            BookOrder order = new BookOrder();
            OrderItem item1 = new OrderItem("B001", "Title", 10.00, 3);
            OrderItem item2 = new OrderItem("B001", "Title", 10.00, 4);

            // Act
            order.AddItem(item1);
            order.AddItem(item2);

            // Assert
            // 1. Should only have one entry for B001
            Assert.AreEqual(1, order.OrderItemList.Count);
            // 2. Quantity should be merged (3 + 4 = 7)
            Assert.AreEqual(7, order.OrderItemList[0].Quantity);
        }

        [TestMethod]
        public void TestGetOrderTotal_MultipleItems()
        {
            // Arrange: Setup order and items
            BookOrder order = new BookOrder();
            // Item 1: 5 quantity @ $10.00 = $50.00
            OrderItem item1 = new OrderItem("B001", "Book A", 10.00, 5);
            // Item 2: 1 quantity @ $15.00 = $15.00
            OrderItem item2 = new OrderItem("B002", "Book B", 15.00, 1);

            // Act
            order.AddItem(item1);
            order.AddItem(item2);
            double total = order.GetOrderTotal();

            // Assert: Total should be 50.00 + 15.00 = 65.00
            Assert.AreEqual(65.00, total);
        }
    }
}
