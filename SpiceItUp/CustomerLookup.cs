using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerLookup
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static bool exit = false;
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
                        SearchByFirstName();
                        break;
                    case 2:
                        SearchByLastName();
                        break;
                    case 3:
                        exit = true;
                        break;
                }
            }
        }

        private static void SearchByFirstName()
        {
            while (true)
            {
                Console.WriteLine("Enter a first name:");
                string? firstName = Console.ReadLine();
                if (firstName == null)
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    int test = 0;
                    try
                    {
                        using SqlConnection connection = new(connectionString);

                        Console.WriteLine("==============================");
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "=======", "=========", "=========", "============", "============"));

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
                    catch (Exception)
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    if (test == 0)
                        Console.WriteLine("There are no customers with that first name. Please try again.");
                    break;
                }
            }
        }

        private static void SearchByLastName()
        {
            while (true)
            {
                Console.WriteLine("Enter a last name:");
                string? lastName = Console.ReadLine();
                if (lastName == null)
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    int test = 0;
                    try
                    {
                        using SqlConnection connection = new(connectionString);

                        Console.WriteLine("==============================");
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
                        Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                                "=======", "=========", "=========", "============", "============"));

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
                    catch (Exception)
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    if (test == 0)
                        Console.WriteLine("There are no customers with that last name. Please try again.");
                    break;
                }
            }
        }
    }
}
