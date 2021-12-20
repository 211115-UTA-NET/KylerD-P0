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
        private static bool exit = false;
        private static int userID;

        private static List<int> itemIDList = new List<int>();
        private static List<string> itemNameList = new List<string>();
        private static List<int> inStockList = new List<int>();
        private static List<decimal> priceList = new List<decimal>();

        private static List<int> customerItemID = new List<int>();
        private static List<string> customerItemName = new List<string>();
        private static List<int> customerQuantity = new List<int>();
        private static List<decimal> customerPrice = new List<decimal>();

        public static void StoreSelection(int myUserID)
        {
            userID = myUserID;
            exit = false;
            while (exit == false)
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
            while (exit == false)
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
                while (exit == false)
                {
                    int itemToAdd;
                    string? adding = Console.ReadLine();
                    bool validEntry = int.TryParse(adding, out itemToAdd);
                    itemToAdd--;
                    if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] != 0)
                    {
                        Console.WriteLine("Enter a quantity to add to cart:");
                        while (exit == false)
                        {
                            string? quantityString = Console.ReadLine();
                            int quantity;
                            bool validQuantity = int.TryParse(quantityString, out quantity);
                            bool failedEntry = false;
                            bool sameEntry = false;
                            bool maxQuantity = false;
                            if (validQuantity == true && quantity > 0 && quantity < 11)
                            {
                                for (int j = 0; j < customerItemID.Count; j++)
                                {
                                    itemToAdd++;
                                    if (itemToAdd == customerItemID[j])
                                    {
                                        itemToAdd--;
                                        sameEntry = true;
                                        int oldQuantity = customerQuantity[j];
                                        int finalQuantity = oldQuantity + quantity;
                                        if (oldQuantity == 10)
                                        {
                                            Console.WriteLine("You already have 10 of this item. Please pick a new item");
                                            maxQuantity = true;
                                            break;
                                        }
                                        else if (finalQuantity > 10)
                                        {
                                            Console.WriteLine($"You already have {oldQuantity} of this item in your cart. This cannot exceed 10 of this item. \nPlease enter a new quantity:");
                                            failedEntry = true;
                                            break;
                                        }
                                        else
                                        {
                                            customerQuantity[j] = customerQuantity[j] + quantity;
                                            decimal newPrice = priceList[itemToAdd] * quantity;
                                            customerPrice[j] = newPrice + customerPrice[j];
                                            inStockList[itemToAdd] = inStockList[itemToAdd] - quantity;
                                            break;
                                        }
                                    }
                                    itemToAdd--;
                                }

                                if (sameEntry == false && failedEntry == false)
                                {
                                    customerItemID.Add(itemIDList[itemToAdd]);
                                    customerItemName.Add(itemNameList[itemToAdd]);
                                    customerQuantity.Add(quantity);
                                    customerPrice.Add(priceList[itemToAdd] * quantity);
                                    inStockList[itemToAdd] = inStockList[itemToAdd] - quantity;
                                    break;
                                }
                                else if (maxQuantity == true)
                                    break;
                                else if (failedEntry == false)
                                    break;
                            }
                            else if (validQuantity == true && quantity > inStockList[itemToAdd])
                                Console.WriteLine("This store does not have enough in stock for that quantity. \nPlease enter a new quantity:");
                            else if (validQuantity == true && quantity < 1)
                                Console.WriteLine("You cannot have a quantity less than 1. \nPlease enter a new quantity:");
                            else
                                Console.WriteLine("Sorry, quantities are limited to 10 per item. \nPlease enter a new quantity.");
                        }
                        break;
                    }
                    else if (validEntry == true && itemToAdd < -1 || itemToAdd >= itemIDList.Count)
                        Console.WriteLine("Invalid item ID. \nPlease enter a new item ID:");
                    else if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] == 0)
                        Console.WriteLine("We are currently out of this item. \nPlease enter a new item ID:");
                    else if (validEntry == true && itemToAdd == -1)
                    {
                        if (customerItemID.Count == 0)
                            Console.WriteLine("Your cart is empty. Enter a valid Item ID to create a cart:");
                        else
                        {
                            ViewCustomerCart();
                            break;
                        }
                    }
                    else if (adding == "EXIT")
                    {
                        itemIDList.Clear();
                        itemNameList.Clear();
                        inStockList.Clear();
                        priceList.Clear();
                        customerItemID.Clear();
                        customerItemName.Clear();
                        customerQuantity.Clear();
                        customerPrice.Clear();
                        exit = true;
                        break;
                    }
                    else
                        Console.WriteLine("There was an error. \nPlease enter a new item ID:");
                }
            }
        }

        private static void RemoveFromCart()
        {
            while (exit == false)
            {
                if (customerItemID.Count == 0)
                {
                    Console.WriteLine("Your cart is now empty.");
                    break;
                }
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
                Console.WriteLine("=========================");
                Console.WriteLine("Enter an Item ID to remove it from your cart, or enter 0 to view your cart.");
                Console.WriteLine("Type 'EXIT' to cancel your order and return to your account menu.");
                while (exit == false)
                {
                    int itemToRemove;
                    string? removing = Console.ReadLine();
                    bool validEntry = int.TryParse(removing, out itemToRemove);
                    itemToRemove--;
                    if (validEntry == true && itemToRemove > -1)
                    {
                        bool failedEntry = true;
                        Console.WriteLine("Enter a quantity to remove from your cart:");
                        while (failedEntry == true)
                        {
                            string? quantityString = Console.ReadLine();
                            int quantity;
                            bool validQuantity = int.TryParse(quantityString, out quantity);
                            if (validQuantity == true && quantity > 0 && quantity < 11)
                            {
                                itemToRemove++;
                                int itemRemoved = 0;
                                for (int j = 0; j < customerItemID.Count; j++)
                                {
                                    if (itemToRemove == customerItemID[j])
                                    {
                                        itemToRemove--;
                                        int oldQuantity = customerQuantity[j];
                                        int finalQuantity = oldQuantity - quantity;
                                        if (finalQuantity < 0)
                                        {
                                            Console.WriteLine("You cannot remove more than what you have. Please enter a new quantity:");
                                            break;
                                        }
                                        else if (finalQuantity == 0)
                                        {
                                            failedEntry = false;
                                            customerItemID.RemoveAt(j);
                                            customerItemName.RemoveAt(j);
                                            customerQuantity.RemoveAt(j);
                                            customerPrice.RemoveAt(j);
                                            inStockList[itemToRemove] = inStockList[itemToRemove] + quantity;
                                            break;
                                        }
                                        else
                                        {
                                            failedEntry = false;
                                            decimal itemPrice = customerPrice[j] / customerQuantity[j];
                                            customerQuantity[j] = customerQuantity[j] - quantity;
                                            decimal newPrice = itemPrice * customerQuantity[j];
                                            customerPrice[j] = newPrice;
                                            inStockList[itemToRemove] = inStockList[itemToRemove] + quantity;
                                            break;
                                        }
                                    }
                                    else
                                        itemRemoved++;
                                }
                                if (itemRemoved == customerItemID.Count)
                                    Console.WriteLine("We could not find that item. Please try again.");
                                break;
                            }
                            else if (validQuantity == true && quantity <= 0)
                                Console.WriteLine("You cannot remove a quantity less than 1. \nPlease enter a new quantity to remove:");
                            else if (validQuantity == true && quantity > 10)
                                Console.WriteLine("You cannot remove a quantity more than 10. \nPlease enter a new quantity to remove:");
                            else
                                Console.WriteLine("There was an error. \nPlease enter a new quantity to remove:");
                        }
                        break;
                    }
                    else if (validEntry == true && itemToRemove == -1)
                        ViewCustomerCart();
                    else if (removing == "EXIT")
                    {
                        itemIDList.Clear();
                        itemNameList.Clear();
                        inStockList.Clear();
                        priceList.Clear();
                        customerItemID.Clear();
                        customerItemName.Clear();
                        customerQuantity.Clear();
                        customerPrice.Clear();
                        exit = true;
                        break;
                    }
                    else
                        Console.WriteLine("There was an error. \nPlease enter a new item ID:");
                }
            }
        }

        private static void FinalizeTransaction()
        {
            StringBuilder createTransID = new StringBuilder();
            Enumerable
               .Range(65, 26)
               .Select(e => ((char)e).ToString())
               .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
               .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
               .OrderBy(e => Guid.NewGuid())
               .Take(11)
               .ToList().ForEach(e => createTransID.Append(e));
            string transID = createTransID.ToString();

            using SqlConnection connection = new(connectionString);
            try
            {
                for (int i = 0; i < itemIDList.Count; i++)
                {
                    connection.Open();
                    string updateStoreInv = "UPDATE StoreInventory SET InStock = @stock WHERE StoreID = @storeID AND ItemID = @itemID;";
                    using SqlCommand newStoreInv = new(updateStoreInv, connection);
                    newStoreInv.Parameters.Add("@stock", System.Data.SqlDbType.Int).Value = inStockList[i];
                    newStoreInv.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
                    newStoreInv.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = itemIDList[i];
                    newStoreInv.ExecuteNonQuery();
                    connection.Close();
                }

                connection.Open();
                string addTransHistory = "INSERT TransactionHistory (TransactionID, UserID, StoreID, IsStoreOrder, Timestamp) " +
                    "VALUES (@transID, @userID, @storeID, @isStoreOrder, @timestamp);";
                using SqlCommand newTransHistory = new(addTransHistory, connection);
                DateTime now = DateTime.Now;
                string dateTime = now.ToString("F");
                newTransHistory.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
                newTransHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
                newTransHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
                newTransHistory.Parameters.Add("@isStoreOrder", System.Data.SqlDbType.VarChar).Value = "FALSE";
                newTransHistory.Parameters.Add("@timestamp", System.Data.SqlDbType.NVarChar).Value = dateTime;
                newTransHistory.ExecuteNonQuery();
                connection.Close();

                for (int i = 0; i < customerItemID.Count; i++)
                {
                    connection.Open();
                    string addTransDetails = "INSERT CustomerTransactionDetails (TransactionID, ItemID, Quantity, Price) " +
                        "VALUES (@transID, @itemID, @quantity, @price);";
                    using SqlCommand newTransDetails = new(addTransDetails, connection);
                    newTransDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
                    newTransDetails.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = customerItemID[i];
                    newTransDetails.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = customerQuantity[i];
                    newTransDetails.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = customerPrice[i];
                    newTransDetails.ExecuteNonQuery();
                    connection.Close();
                }

                Console.WriteLine("Your order was successful!");
                Console.WriteLine($"Your transaction ID number is: {transID}");

                itemIDList.Clear();
                itemNameList.Clear();
                inStockList.Clear();
                priceList.Clear();
                customerItemID.Clear();
                customerItemName.Clear();
                customerQuantity.Clear();
                customerPrice.Clear();
                exit = true;
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error processing your order. Our stock may have changed.");
            }
        }

        private static void ViewCustomerCart()
        {
            Console.WriteLine($"Cart for store {storeEntry}: {storeName}");
            Console.WriteLine("=========================================");
            Console.WriteLine("Item ID\t Item Name\t Quantity\t Price");
            Console.WriteLine("=======\t =========\t ========\t =====");
            decimal totalPrice = 0;
            for (int i = 0; i < customerItemID.Count; i++)
            {
                string price = String.Format("{0:0.00}", customerPrice[i]);
                Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                    customerItemID[i], customerItemName[i], customerQuantity[i], $"${price}"));
                totalPrice = totalPrice + customerPrice[i];
            }
            string stringTotalPrice = String.Format("{0:0.00}", totalPrice);
            Console.WriteLine("=========================");
            Console.WriteLine($"Your total is ${stringTotalPrice}");
            Console.WriteLine("=========================");
            Console.WriteLine("How would you like to continue?");
            Console.WriteLine("1: Add more items to my cart \n2: Remove items from my cart \n3: Finalize transaction and checkout \n4: Cancel transaction and return to menu");

            int userEntry;
            while (true) //Test to ensure user entry is valid
            {
                string? mySelection = Console.ReadLine();
                bool validEntry = int.TryParse(mySelection, out userEntry);
                if (validEntry == true && userEntry >= 1 && userEntry <= 4)
                {
                    if (userEntry == 3 && customerItemID.Count == 0)
                        Console.WriteLine("You cannot checkout with an empty cart. Please select another option:");
                    else
                        break; //Break when valid
                }
                else
                    Console.WriteLine("Invalid selection. Please try again.");
            }

            switch (userEntry)
            {
                case 1:
                    AddToCart();
                    break;
                case 2:
                    RemoveFromCart();
                    break;
                case 3:
                    Console.WriteLine("Are you sure? (Y/N)");
                    string? check = Console.ReadLine();
                    if ("Y" == check?.ToUpper())
                        FinalizeTransaction();
                    break;
                case 4:
                    itemIDList.Clear();
                    itemNameList.Clear();
                    inStockList.Clear();
                    priceList.Clear();
                    customerItemID.Clear();
                    customerItemName.Clear();
                    customerQuantity.Clear();
                    customerPrice.Clear();
                    exit = true;
                    break;
            }
        }
    }
}
