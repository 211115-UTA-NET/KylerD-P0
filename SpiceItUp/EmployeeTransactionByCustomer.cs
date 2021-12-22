using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can lookup a customers transaction based on a list of customers
    /// </summary>
    public class EmployeeTransactionByCustomer : IViewTransaction
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static bool exit = false;
        private static bool goBack = false;
        private static int userEntry;
        private static int userEntry2;

        /// <summary>
        /// Customer's basic info is stored in lists
        /// </summary>
        private static List<int> customerIDList = new List<int>();
        private static List<string> customerFirstNameList = new List<string>();
        private static List<string> customerLastNameList = new List<string>();
        public static List<string> transList = new List<string>();

        /// <summary>
        /// The database prints off a list of customers for the employee to choose from
        /// </summary>
        public static void SelectACustomer()
        {
            exit = false;
            while (exit == false)
            {
                customerIDList.Clear();
                customerFirstNameList.Clear();
                customerLastNameList.Clear();

                using SqlConnection connection = new(connectionString);

                //Format our customer list
                Console.WriteLine("Here is the customer list:");
                Console.WriteLine("==============================");
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                        "Entry", "First Name", "Last Name", "Phone Number"));
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                        "=====", "==========", "=========", "============"));

                //Pull a list of customers from the database
                connection.Open();
                string getCustomerList = "SELECT UserID, FirstName, LastName, PhoneNumber FROM UserInformation WHERE IsEmployee = 'FALSE';";
                using SqlCommand readCustomerList = new(getCustomerList, connection);
                using SqlDataReader readCustomers = readCustomerList.ExecuteReader();
                int entry = 1;
                while (readCustomers.Read())
                {
                    customerIDList.Add(readCustomers.GetInt32(0));
                    customerFirstNameList.Add(readCustomers.GetString(1));
                    customerLastNameList.Add(readCustomers.GetString(2));
                    Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                        entry, readCustomers.GetString(1), readCustomers.GetString(2), readCustomers.GetInt64(3)));
                    entry++;
                }
                connection.Close();

                Console.WriteLine($"To view a customer's order history, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to exit.");

                while (true)
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (customerIDList.Count >= userEntry && userEntry > 0) //If employee chooses a valid customer
                    {
                        userEntry--;
                        TransactionHistory(userEntry); //Print customer's transaction history
                        break;
                    }
                    else if (userEntry == 0) //If customer wishes to return to account main menu
                    {
                        exit = true;
                        break;
                    }
                    else //If we get an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }

        /// <summary>
        /// Once the employee chooses a customer, that customer's transaction list is printed off.
        /// The employee can choose to view a transaction in more details if they wish
        /// </summary>
        public static void TransactionHistory(int myEntry)
        {
            goBack = false;

            while (goBack == false)
            {
                transList.Clear();

                using SqlConnection connection = new(connectionString);

                //Format our transaction list
                Console.WriteLine($"Here is {customerFirstNameList[myEntry]} {customerLastNameList[myEntry]}'s History:");
                Console.WriteLine("==============================");
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        "Entry", "Transaction ID", "Store ID", "Total Price"));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        "=====", "==============", "========", "==========="));

                //Get transaction list from database and print
                connection.Open();
                string getOrderHistory = "SELECT TransactionHistory.TransactionID, TransactionHistory.StoreID, SUM(CustomerTransactionDetails.Price) " +
                    "FROM TransactionHistory JOIN CustomerTransactionDetails " +
                    "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                    "WHERE TransactionHistory.UserID = @userID GROUP BY TransactionHistory.TransactionID, TransactionHistory.StoreID;";
                using SqlCommand orderHistory = new(getOrderHistory, connection);
                orderHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = customerIDList[myEntry];
                using SqlDataReader reader = orderHistory.ExecuteReader();
                int entry = 1;
                while (reader.Read())
                {
                    transList.Add(reader.GetString(0));
                    string price = String.Format("{0:0.00}", reader.GetDecimal(2));
                    Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        entry, reader.GetString(0), reader.GetInt32(1), $"${price}"));
                    entry++;
                }
                connection.Close();

                if (transList.Count == 0) //If the customer does not have any transactions to view, return to customer selection
                {
                    Console.WriteLine("Going back. This customer does not have a transaction history.");
                    break;
                }
                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to go back.");

                while (true) //The employee is prompted to choose a transaction to view in more detail
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry2);
                    if (transList.Count >= userEntry2 && userEntry2 > 0) //If employee chooses a transaction
                    {
                        DetailedTransaction(); //View the transaction in more detail
                        break;
                    }
                    else if (userEntry2 == 0) //Return to customer list
                    {
                        goBack = true;
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }

        /// <summary>
        /// A customer's full transaction details are printed off to the employee
        /// </summary>
        public static void DetailedTransaction()
        {
            int entryList = userEntry2 - 1;
            string detailedTransID = transList[entryList];

            using SqlConnection connection = new(connectionString);

            //Print off details reguarding selected transaction
            connection.Open();
            string getOpening = "SELECT TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, " +
                "TransactionHistory.Timestamp, SUM(CustomerTransactionDetails.Price) " +
                "FROM TransactionHistory JOIN StoreInfo " +
                "ON TransactionHistory.StoreID = StoreInfo.StoreID " +
                "JOIN CustomerTransactionDetails " +
                "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                "WHERE TransactionHistory.UserID = @userID AND TransactionHistory.TransactionID = @transID " +
                "GROUP BY TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, TransactionHistory.Timestamp;";
            using SqlCommand readOpening = new(getOpening, connection);
            readOpening.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = customerIDList[userEntry];
            readOpening.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader myReader = readOpening.ExecuteReader();
            while (myReader.Read())
            {
                string price = String.Format("{0:0.00}", myReader.GetDecimal(4));
                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {myReader.GetString(0)}");
                Console.WriteLine($"Store {myReader.GetInt32(1)}: {myReader.GetString(2)}");
                Console.WriteLine($"Time: {myReader.GetString(3)}");
                Console.WriteLine($"Total: ${price}");
                Console.WriteLine("==============================");
            }
            connection.Close();

            //Format transaction
            Console.WriteLine("Item Name\t Quantity\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Print off items that were included in transaction
            connection.Open();
            string getDetails = "SELECT ItemDetails.ItemName, CustomerTransactionDetails.Quantity, CustomerTransactionDetails.Price " +
                "FROM CustomerTransactionDetails JOIN ItemDetails ON CustomerTransactionDetails.ItemID = ItemDetails.ItemID " +
                "WHERE CustomerTransactionDetails.TransactionID = @transID;";
            using SqlCommand readDetails = new(getDetails, connection);
            readDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader detailReader = readDetails.ExecuteReader();
            while (detailReader.Read())
            {
                string price = String.Format("{0:0.00}", detailReader.GetDecimal(2));
                Console.WriteLine(String.Format("{0, -16} {1, -16} {2, -16}",
                detailReader.GetString(0), detailReader.GetInt32(1), $"${price}"));
            }
            Console.WriteLine("==============================");
            connection.Close();
            Console.WriteLine("Press 'ENTER' to continue...");
            Console.ReadLine();
            //Employee is returned to transaction list
        }
    }
}
