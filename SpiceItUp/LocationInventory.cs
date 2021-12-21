using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Customers and employees can view a store's inventory.
    /// Customers can view the store's inventory before they begin an order.
    /// </summary>
    public class LocationInventory
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");
        private static int storeEntry;

        /// <summary>
        /// Print off a list of stores by pulling store information from the database.
        /// The user can then sleect a store to view it's inventory
        /// </summary>
        public static void StoreSelection()
        {
            while (true)
            {
                Console.WriteLine("Enter a store number to view their inventory:");

                using SqlConnection connection = new(connectionString);

                //Get list of stores from database
                connection.Open();
                string getStoreInfo = "SELECT * FROM StoreInfo ORDER BY StoreID;";
                using SqlCommand readStoreInfo = new(getStoreInfo, connection);
                using SqlDataReader reader = readStoreInfo.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Store {reader.GetInt32(0)}: {reader.GetString(1)}");
                }
                connection.Close();

                while (true) //Test to ensure user entry is valid
                {
                    string? storeSelection = Console.ReadLine();
                    bool validEntry = int.TryParse(storeSelection, out storeEntry);
                    if (validEntry == true && storeEntry > 100 && storeEntry < 105)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                try
                {
                    PullStoreInfo(); //Can we pull the store's inventory information based on user input?
                }
                catch (Exception) //If we fail to pull store inventory
                {
                    Console.WriteLine("Unable to pull store information");
                }

                Console.WriteLine("Would you like to check another store's inventory? (Y/N)"); //If customer wishes to check another store's inventory, reprint store lists
                string? checkNewStore = Console.ReadLine();
                if ("Y" != checkNewStore?.ToUpper()) //If customer does not wish to check another store's inventory, break the loop and return to account main menu
                    break;
            }
        }

        /// <summary>
        /// We will attempt to pull the inventory from entered store
        /// Store inventory is pulled from database and formatted accordingly
        /// </summary>
        public static void PullStoreInfo()
        {
            using SqlConnection connection = new(connectionString);

            //Pull the selected store information
            connection.Open();
            string getSelectedStore = $"SELECT * FROM StoreInfo WHERE StoreID = @storeID;";
            using SqlCommand readSelectedStore = new(getSelectedStore, connection);
            readSelectedStore.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readStore = readSelectedStore.ExecuteReader();
            while(readStore.Read())
            {
                Console.WriteLine($"Inventory for store {readStore.GetInt32(0)}: {readStore.GetString(1)}");
            }
            connection.Close();

            //Formatting
            Console.WriteLine("=================================");
            Console.WriteLine("Item Name\t In Stock\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Pull and print the store's inventory
            connection.Open();
            string getStoreInventory = "SELECT ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice " +
                "FROM StoreInventory JOIN ItemDetails " +
                "ON StoreInventory.ItemID = ItemDetails.ItemID " +
                "WHERE StoreInventory.StoreID = @storeID ORDER BY ItemDetails.ItemName;";
            using SqlCommand readStoreInventory = new(getStoreInventory, connection);
            readStoreInventory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readInventory = readStoreInventory.ExecuteReader();
            while (readInventory.Read())
            {
                decimal itemPrice = readInventory.GetDecimal(2);
                string price = String.Format("{0:0.00}", itemPrice);
                Console.WriteLine(String.Format("{0, -16} {1, -15} {2, -16}", readInventory.GetString(0), readInventory.GetInt32(1), $"${price}"));
            }
            connection.Close();
        }
    }
}