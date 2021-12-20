using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerOrderHistory
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static int userID;
        public static bool exit = false;
        private static List<string> transList = new List<string>();
        private static int userEntry;

        public static void CustomerTransactionHistory(int myUserID)
        {
            userID = myUserID;
            exit = false;
            while (exit == false)
            {
                using SqlConnection connection = new(connectionString);

                Console.WriteLine("Here is your order History:");
                Console.WriteLine("==============================");
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        "Entry", "Transaction ID", "Store ID", "Total Price"));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        "=====", "==============", "========", "==========="));

                connection.Open();
                string getOrderHistory = "SELECT TransactionHistory.TransactionID, TransactionHistory.StoreID, SUM(CustomerTransactionDetails.Price) " +
                    "FROM TransactionHistory JOIN CustomerTransactionDetails " +
                    "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                    "WHERE TransactionHistory.UserID = @userID GROUP BY TransactionHistory.TransactionID, TransactionHistory.StoreID;";
                using SqlCommand orderHistory = new(getOrderHistory, connection);
                orderHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
                using SqlDataReader reader = orderHistory.ExecuteReader();
                int entry = 1;
                while(reader.Read())
                {
                    transList.Add(reader.GetString(0));
                    string price = String.Format("{0:0.00}", reader.GetDecimal(2));
                    Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                        entry, reader.GetString(0), reader.GetInt32(1), $"${price}"));
                    entry++;
                }
                connection.Close();

                if (transList.Count == 0)
                {
                    Console.WriteLine("Returning to your menu. You do not have a transaction history.");
                    break;
                }
                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to exit.");

                while (true)
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (transList.Count >= userEntry && userEntry > 0)
                    {
                        DetailedTransaction();
                        break;
                    }
                    else if (userEntry == 0)
                    {
                        exit = true;
                        break;
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }

        public static void DetailedTransaction()
        {
            int entryList = userEntry - 1;
            string detailedTransID = transList[entryList];

            using SqlConnection connection = new(connectionString);
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
            readOpening.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
            readOpening.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader myReader = readOpening.ExecuteReader();
            while(myReader.Read())
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

            Console.WriteLine("Item Name\t Quantity\t Price");
            Console.WriteLine("=========\t ========\t =====");

            connection.Open();
            string getDetails = "SELECT ItemDetails.ItemName, CustomerTransactionDetails.Quantity, CustomerTransactionDetails.Price " +
                "FROM CustomerTransactionDetails JOIN ItemDetails ON CustomerTransactionDetails.ItemID = ItemDetails.ItemID " +
                "WHERE CustomerTransactionDetails.TransactionID = @transID;";
            using SqlCommand readDetails = new(getDetails, connection);
            readDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader detailReader = readDetails.ExecuteReader();
            while(detailReader.Read())
            {
                string price = String.Format("{0:0.00}", detailReader.GetDecimal(2));
                Console.WriteLine(String.Format("{0, -16} {1, -16} {2, -16}",
                detailReader.GetString(0), detailReader.GetInt32(1), $"${price}"));
            }
            Console.WriteLine("==============================");
            connection.Close();
            Console.WriteLine("Press 'ENTER' to continue...");
            Console.ReadLine();
        }
    }
}
