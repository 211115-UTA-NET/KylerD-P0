using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerOrder
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");
        private static int storeEntry;
        private static string? storeName;

        private static List<int> itemIDList = new List<int>();
        private static List<string> itemNameList = new List<string>();
        private static List<int> inStockList = new List<int>();
        private static List<decimal> priceList = new List<decimal>();

        private static List<int> customerItemID = new List<int>();
        private static List<string> customerItemName = new List<string>();
        private static List<int> customerQuantity = new List<int>();
        private static List<decimal> customerPrice = new List<decimal>();

        public static void StoreSelection()
        {
            while (true)
            {
                Console.WriteLine("Enter a store number to begin shopping:");

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
                    if (validEntry == true && storeEntry > 100)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                try
                {
                    PullStoreInfo();
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to pull store information.");
                }

                AddToCart();
            }
        }

        private static void PullStoreInfo()
        {
            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getSelectedStore = $"SELECT * FROM StoreInfo WHERE StoreID = @storeID;";
            using SqlCommand readSelectedStore = new(getSelectedStore, connection);
            readSelectedStore.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readStore = readSelectedStore.ExecuteReader();
            while (readStore.Read())
            {
                storeName = readStore.GetString(1);
            }
            connection.Close();

            connection.Open();
            string getStoreInventory = "SELECT ItemDetails.ItemID, ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice " +
                "FROM StoreInventory JOIN ItemDetails " +
                "ON StoreInventory.ItemID = ItemDetails.ItemID " +
                "WHERE StoreInventory.StoreID = @storeID ORDER BY ItemDetails.ItemID;";
            using SqlCommand readStoreInventory = new(getStoreInventory, connection);
            readStoreInventory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readInventory = readStoreInventory.ExecuteReader();
            while (readInventory.Read())
            {
                decimal itemPrice = readInventory.GetDecimal(3);
                itemIDList.Add(readInventory.GetInt32(0));
                itemNameList.Add(readInventory.GetString(1));
                inStockList.Add(readInventory.GetInt32(2));
                priceList.Add(itemPrice);
            }
            connection.Close();
        }

        private static void AddToCart()
        {
            while (true)
            {
                Console.WriteLine($"Store {storeEntry}: {storeName}");
                Console.WriteLine("================================");
                Console.WriteLine("Item ID\t Item Name\t In Stock\t Price");
                Console.WriteLine("=======\t =========\t ========\t =====");
                for (int i = 0; i < itemIDList.Count; i++)
                {
                    string price = String.Format("{0:0.00}", priceList[i]);
                    Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                        itemIDList[i], itemNameList[i], inStockList[i], $"${price}"));
                }
                Console.WriteLine("Enter an Item ID to add to your cart, or enter 0 to view your cart.");
                Console.WriteLine("Type 'EXIT' to cancel your order and return to your account menu.");
                while (true)
                {
                    int itemToAdd;
                    string? adding = Console.ReadLine();
                    bool validEntry = int.TryParse(adding, out itemToAdd);
                    itemToAdd--;
                    if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] != 0)
                    {
                        Console.WriteLine("Enter a quantity to add to cart:");
                        while (true)
                        {
                            string? quantityString = Console.ReadLine();
                            int quantity;
                            bool validQuantity = int.TryParse(quantityString, out quantity);
                            if (validQuantity == true && quantity > 0 && quantity < 11)
                            {
                                customerItemID.Add(itemIDList[itemToAdd]);
                                customerItemName.Add(itemNameList[itemToAdd]);
                                customerQuantity.Add(inStockList[quantity]);
                                customerPrice.Add(priceList[itemToAdd] * quantity);
                                inStockList[itemToAdd] = inStockList[itemToAdd] - quantity;
                                break;
                            }
                            else if (validQuantity == true && quantity > inStockList[itemToAdd])
                                Console.WriteLine("This store does not have enough in stock for that quantity. \nPlease enter a new quantity:");
                            else
                                Console.WriteLine("Sorry, quantities are limited to 10 per item. \nPlease enter a new quantity.");
                        }
                    }
                    else if (validEntry == true && itemToAdd < -1 && itemToAdd >= itemIDList.Count)
                        Console.WriteLine("Invalid item ID. \nPlease enter a new item ID:");
                    else if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] == 0)
                        Console.WriteLine("We are currently out of this item. \nPlease enter a new item ID:");
                    else
                        Console.WriteLine("There was an error. \nPlease enter a new item ID:");
                }
            }
        }

        private static void ViewCustomerCart()
        {
            Console.WriteLine($"Cart for store {storeEntry}: {storeName}");
            Console.WriteLine("=========================================");
            Console.WriteLine("Item ID\t Item Name\t Quantity\t Price");
            Console.WriteLine("=======\t =========\t ========\t =====");
            for (int i = 0; i < customerItemID.Count; i++)
            {
                string price = String.Format("{0:0.00}", customerPrice[i]);
                Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                    customerItemID[i], customerItemName[i], customerQuantity[i], $"${price}"));
            }
        }
    }
}
