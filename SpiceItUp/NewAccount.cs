using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace SpiceItUp
{
    public class NewAccount
    {
        private static string? firstName;
        private static string? lastName;
        private static string? phoneNumber;
        private static string? newUsername;
        private static string? newPassword;

        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");


        public static void CreateAnAccount()
        {
            Console.WriteLine("Lets create an account! Type 'EXIT' at any time to return to the home screen.");
            CustomerInformation();
            CustomerLoginInformation();
            try
            {
                AddNewCustomer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Looks like there was an error. Please try again.");
            }
            SpiceItUp.Program.Main();
        }

        public static void CustomerInformation()
        {
            while (true) //Create an account FIRST name
            {
                Console.WriteLine("Please enter your first name:");
                firstName = Console.ReadLine();
                if (firstName == "")
                    Console.WriteLine("No entry. Please try again");
                else if (firstName == "EXIT")
                    SpiceItUp.Program.Main();
                else
                {
                    Console.WriteLine($"Is {firstName} your first name? (Y/N)");
                    string? validFirstName = Console.ReadLine();
                    if (validFirstName != "" && "Y" == validFirstName.ToUpper())
                        break;
                }
            }

            while (true) //Create an account LAST name
            {
                Console.WriteLine("Please enter your last name:");
                lastName = Console.ReadLine();
                if (lastName == "")
                    Console.WriteLine("No entry. Please try again");
                else if (lastName == "EXIT")
                    SpiceItUp.Program.Main();
                else
                {
                    Console.WriteLine($"Is {lastName} your last name? (Y/N)");
                    string? validLastName = Console.ReadLine();
                    if (validLastName != "" && "Y" == validLastName.ToUpper())
                        break;
                }
            }

            while (true) //Create a phone number
            {
                Console.WriteLine("Please enter a valid phone number:");
                phoneNumber = Console.ReadLine();
                if (phoneNumber == "")
                    Console.WriteLine("No entry. Please try again");
                else if (phoneNumber == "EXIT")
                    SpiceItUp.Program.Main();
                else if (phoneNumber.Length < 9)
                {
                    Console.WriteLine("The phone number you entered is not long enough. Please enter a new phone number.");
                }
                else
                {
                    Console.WriteLine($"Is {phoneNumber} your valid phone number? (Y/N)");
                    string? validPhoneNumber = Console.ReadLine();
                    if (validPhoneNumber != "" && "Y" == validPhoneNumber.ToUpper())
                        break;
                    else if (validPhoneNumber == "EXIT")
                        SpiceItUp.Program.Main();
                }
            }
        }

        public static void CustomerLoginInformation()
        {
            while (true) //Create an account username
            {
                Console.WriteLine("Please enter your username:");
                newUsername = Console.ReadLine();
                if (newUsername == "")
                    Console.WriteLine("No entry. Please try again");
                else if (firstName == "EXIT")
                    SpiceItUp.Program.Main();
                else
                {
                    Console.WriteLine($"Is {newUsername} your username? (Y/N)");
                    string? validUsername = Console.ReadLine();
                    if (validUsername != "" && "Y" == validUsername.ToUpper())
                        break;
                    else if (validUsername == "EXIT")
                        SpiceItUp.Program.Main();
                }
            }

            while (true) //Create an account password
            {
                Console.WriteLine("Please enter your password (must be at least 8 characters long):");
                newPassword = Console.ReadLine();
                if (newPassword == "")
                    Console.WriteLine("No entry. Please try again");
                else if (newPassword == "EXIT")
                    SpiceItUp.Program.Main();
                else if (newPassword.Length < 8)
                {
                    Console.WriteLine("Password is not long enough. Please enter a new password.");
                }
                else
                {
                    Console.WriteLine("Please re-enter your password for verification:");
                    string? validPassword = Console.ReadLine();
                    if (validPassword != "" && newPassword == validPassword && newPassword.Length >= 8)
                        break;
                    else if (validPassword == "EXIT")
                        SpiceItUp.Program.Main();
                    else
                        Console.WriteLine("Passwords did not match. Please try again.");
                }
            }
        }

        public static void AddNewCustomer()
        {
            using SqlConnection connection = new(connectionString);

            // Add the customer's login information to SQL
            connection.Open();
            string addNewLoginManager = $"INSERT LoginManager (Username, \"Password\") VALUES (@username, @password);";
            using SqlCommand newLoginManagerCommand = new(addNewLoginManager, connection);
            newLoginManagerCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = newUsername;
            newLoginManagerCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = newPassword;
            newLoginManagerCommand.ExecuteNonQuery();
            connection.Close();

            // Extract the new ID number that was automatically created
            connection.Open();
            string getNewUserID = $"SELECT UserID FROM LoginManager WHERE Username = @username;";
            using SqlCommand readNewUserID = new(getNewUserID, connection);
            readNewUserID.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = newUsername;
            using SqlDataReader reader = readNewUserID.ExecuteReader();
            int finalIDGrab = 0;
            while(reader.Read())
            {
                finalIDGrab = reader.GetInt32(0);
            }
            connection.Close();

            // Add the customer's information to SQL
            connection.Open();
            string addNewCustomer = $"INSERT Customers (CustomerID, FirstName, LastName, PhoneNumber) VALUES (@customerID, @firstName, @lastName, @phoneNumber);";
            using SqlCommand newCustomerCreationCommand = new(addNewCustomer, connection);
            newCustomerCreationCommand.Parameters.Add("@customerID", System.Data.SqlDbType.Int).Value = finalIDGrab;
            newCustomerCreationCommand.Parameters.Add("@firstName", System.Data.SqlDbType.VarChar).Value = firstName;
            newCustomerCreationCommand.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = lastName;
            newCustomerCreationCommand.Parameters.Add("@phoneNumber", System.Data.SqlDbType.BigInt).Value = phoneNumber;
            newCustomerCreationCommand.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine($"Your account has been created, {firstName}! You may now login!");
        }
    }
}
