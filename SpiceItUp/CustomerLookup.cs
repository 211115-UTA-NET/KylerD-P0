using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can pull a customers information by looking up their name
    /// All information is pulled from the database
    /// </summary>
    public class CustomerLookup
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static bool exit = false;

        public static string? firstName;
        public static string? lastName;
        public static int test = 0;

        /// <summary>
        /// Employee chooses how they would like to lookup the customer (first or last name)
        /// </summary>
        public static void CustomerSearchOptions()
        {
            while (exit == false)
            {
                exit = false;
                Console.WriteLine("Would you like to search by:");
                Console.WriteLine("1: First Name");
                Console.WriteLine("2: Last Name");
                Console.WriteLine("3: Or return to menu?");

                int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && userEntry >= 1 && userEntry <= 3)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1:
                        SearchByFirstName(); //If employee chooses first name
                        break;
                    case 2:
                        SearchByLastName(); //If employee chooses last name
                        break;
                    case 3: //Exit and return to account menu
                        exit = true;
                        break;
                }
            }
        }

        /// <summary>
        /// The employye can search the database by entering a first name
        /// </summary>
        public static void SearchByFirstName()
        {
            while (true)
            {
                Console.WriteLine("Enter a first name:");
                firstName = Console.ReadLine();
                if (firstName == null) //If entry is null
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    test = 0;
                    try //Try to pull customer information
                    {
                        using SqlConnection connection = new(connectionString);

                        Console.WriteLine("==============================");
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "=======", "=========", "=========", "============", "============"));

                        //Pull customer information matching first name entered
                        connection.Open();
                        string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                            "WHERE FirstName = @firstName ORDER BY LastName;";
                        using SqlCommand getCustomer = new(customerSearch, connection);
                        getCustomer.Parameters.Add("@firstName", System.Data.SqlDbType.VarChar).Value = firstName;
                        using SqlDataReader reader = getCustomer.ExecuteReader();
                        while(reader.Read())
                        {
                            test = test + 1;
                            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
                        }
                        Console.WriteLine("==============================");
                        break;
                    }
                    catch (Exception) //If we run into an error while accessing database
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    if (test == 0) //If there are no customers that match the first name entered
                        Console.WriteLine("There are no customers with that first name. Please try again.");
                    break;
                }
            }
        }

        /// <summary>
        /// The employye can search the database by entering a last name
        /// </summary>
        public static void SearchByLastName()
        {
            while (true)
            {
                Console.WriteLine("Enter a last name:");
                lastName = Console.ReadLine();
                if (lastName == null) //If entry is null
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    test = 0;
                    try //Try to pull customer information
                    {
                        using SqlConnection connection = new(connectionString);

                        Console.WriteLine("==============================");
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "=======", "=========", "=========", "============", "============"));

                        //Pull customer information matching last name entered
                        connection.Open();
                        string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                            "WHERE LastName = @lastName ORDER BY LastName;";
                        using SqlCommand getCustomer = new(customerSearch, connection);
                        getCustomer.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = lastName;
                        using SqlDataReader reader = getCustomer.ExecuteReader();
                        while (reader.Read())
                        {
                            test = test + 1;
                            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
                        }
                        Console.WriteLine("==============================");
                        break;
                    }
                    catch (Exception) //If we run into an error while accessing database
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    if (test == 0) //If there are no customers that match the last name entered
                        Console.WriteLine("There are no customers with that last name. Please try again.");
                    break;
                }
            }
        }
    }
}
