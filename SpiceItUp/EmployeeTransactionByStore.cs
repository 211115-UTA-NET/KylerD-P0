using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can lookup a customers transaction based on what store it was ordered from
    /// </summary>
    public class EmployeeTransactionByStore : IViewTransaction
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        /// <summary>
        /// A stores info along with transactions are stored in lists
        /// </summary>
        private static List<int> storeIDList = new List<int>();
        private static List<string> storeNameList = new List<string>();
        public static List<string> transList = new List<string>();

        private static bool exit = false;
        private static int storeEntry;
        private static int userEntry;

        /// <summary>
        /// Employee is given a list of stores.
        /// Employe is then given a list of transactions made at the selected store
        /// </summary>
        public static void StoreSelection()
        {
            exit = false;
            while (true)
            {
                storeIDList.Clear();
                storeNameList.Clear();

                Console.WriteLine("Enter a store number to view the order history:");

                using SqlConnection connection = new(connectionString);

                //Get list of stores from database
                connection.Open();
                string getStoreInfo = "SELECT * FROM StoreInfo ORDER BY StoreID;";
                using SqlCommand readStoreInfo = new(getStoreInfo, connection);
                using SqlDataReader reader = readStoreInfo.ExecuteReader();
                while (reader.Read())
                {
                    storeIDList.Add(reader.GetInt32(0));
                    storeNameList.Add(reader.GetString(1));
                    Console.WriteLine($"Store {reader.GetInt32(0)}: {reader.GetString(1)}");
                }
                connection.Close();

                while (true) //Test to ensure user entry is valid
                {
                    string? storeSelection = Console.ReadLine();
                    bool validEntry = int.TryParse(storeSelection, out storeEntry);
                    if (validEntry == true && storeEntry > 100 && storeEntry < 105) //If selected store is valid
                    {
                        TransactionHistory(storeEntry); //Print off transactions in the database from entered store
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                Console.WriteLine("Would you like to check another store's order history? (Y/N)"); //Employee can shoose another store to view order history from
                string? checkNewStore = Console.ReadLine();
                if ("Y" != checkNewStore?.ToUpper()) //Return to account main menu
                    break;
            }
        }

        /// <summary>
        /// The basic transaction information is printed for the selected store.
        /// The employee has the option to view a transaction more in depth
        /// </summary>
        public static void TransactionHistory(int myEntry)
        {
            int selectedStore = myEntry - 101;
            exit = false;
            while (exit == false)
            {
                transList.Clear();

                using SqlConnection connection = new(connectionString);

                //Format our transactions
                Console.WriteLine($"Order history for store {storeIDList[selectedStore]}: {storeNameList[selectedStore]}");
                Console.WriteLine("==============================");
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                        "Entry", "Transaction ID", "First Name", "Last Name", "Total Price"));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                        "=====", "==============", "==========", "==========", "=========="));

                //Print off transactions at selected store from database
                connection.Open();
                string getOrderHistory = "SELECT TransactionHistory.TransactionID, UserInformation.FirstName, UserInformation.LastName, " +
                    "SUM(CustomerTransactionDetails.Price) " +
                    "FROM TransactionHistory JOIN UserInformation " +
                    "ON TransactionHistory.UserID = UserInformation.UserID " +
                    "JOIN CustomerTransactionDetails " +
                    "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                    "WHERE TransactionHistory.StoreID = @storeID " +
                    "GROUP BY TransactionHistory.TransactionID, UserInformation.FirstName, UserInformation.LastName;";
                using SqlCommand storeOrderHistory = new(getOrderHistory, connection);
                storeOrderHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeIDList[selectedStore];
                using SqlDataReader reader = storeOrderHistory.ExecuteReader();
                int entry = 1;
                while (reader.Read())
                {
                    transList.Add(reader.GetString(0));
                    string price = String.Format("{0:0.00}", reader.GetDecimal(3));
                    Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                        entry, reader.GetString(0), reader.GetString(1), reader.GetString(2), $"${price}"));
                    entry++;
                }
                connection.Close();

                if (transList.Count == 0) //If the store has no transactions, return to store list
                {
                    Console.WriteLine("Going back. This store does not have a transaction history.");
                    break;
                }
                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to go back.");

                while (true) //The employee can now view a store's transaction in more detail, if they wish
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (transList.Count >= userEntry && userEntry > 0) //If a transaction selectionn is valid
                    {
                        DetailedTransaction(); //Pull results of transaction from database and print for employee
                        break;
                    }
                    else if (userEntry == 0) //If employee enters 0, return to store list
                    {
                        exit = true;
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }

        /// <summary>
        /// A transaction is printed off in more detail based on employee selection
        /// </summary>
        public static void DetailedTransaction()
        {
            int entryList = userEntry - 1;
            string detailedTransID = transList[entryList];

            using SqlConnection connection = new(connectionString);

            //Pull transaction details from database and print
            connection.Open();
            string getOpening = "SELECT TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, " +
                "TransactionHistory.Timestamp, UserInformation.FirstName, UserInformation.LastName, SUM(CustomerTransactionDetails.Price) " +
                "FROM TransactionHistory JOIN StoreInfo " +
                "ON TransactionHistory.StoreID = StoreInfo.StoreID " +
                "JOIN CustomerTransactionDetails " +
                "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                "JOIN UserInformation " +
                "ON Userinformation.UserID = TransactionHistory.UserID " +
                "WHERE TransactionHistory.TransactionID = @transID " +
                "GROUP BY TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, TransactionHistory.Timestamp, UserInformation.FirstName, UserInformation.LastName;";
            using SqlCommand readOpening = new(getOpening, connection);
            readOpening.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader myReader = readOpening.ExecuteReader();
            while (myReader.Read())
            {
                string price = String.Format("{0:0.00}", myReader.GetDecimal(6));
                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {myReader.GetString(0)}");
                Console.WriteLine($"Store {myReader.GetInt32(1)}: {myReader.GetString(2)}");
                Console.WriteLine($"Name: {myReader.GetString(4)} {myReader.GetString(5)}");
                Console.WriteLine($"Time: {myReader.GetString(3)}");
                Console.WriteLine($"Total: ${price}");
                Console.WriteLine("==============================");
            }
            connection.Close();

            //Format transaction information
            Console.WriteLine("Item Name\t Quantity\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Print items that were bought in transaction
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
            //Employee is returned to list of transactions at selected store
        }
    }
}
