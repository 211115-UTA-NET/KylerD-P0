using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class NewAccount
    {
        private static string? newName;
        private static string? newUsername;
        private static string? newPassword;

        public static void CreateAnAccount()
        {
            while (true) //Create an account name
            {
                Console.WriteLine("Please enter your name:");
                newName = Console.ReadLine();
                if (newName == "")
                    Console.WriteLine("No entry. Please try again");
                else
                {
                    Console.WriteLine($"Is {newName} your name? (Y/N)");
                    string? validName = Console.ReadLine();
                    if (validName != "" && "Y" == validName.ToUpper())
                        break;
                }
            }

            while (true) //Create an account undername
            {
                Console.WriteLine("Please enter your username:");
                newUsername = Console.ReadLine();
                if (newUsername == "")
                    Console.WriteLine("No entry. Please try again");
                else
                {
                    Console.WriteLine($"Is {newUsername} your username? (Y/N)");
                    string? validUsername = Console.ReadLine();
                    if (validUsername != "" && "Y" == validUsername.ToUpper())
                        break;
                }
            }

            while (true) //Create an account password
            {
                Console.WriteLine("Please enter your password (must be at least 8 characters long):");
                newPassword = Console.ReadLine();
                if (newPassword == "")
                    Console.WriteLine("No entry. Please try again");
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
                    else
                        Console.WriteLine("Passwords did not match. Please try again.");
                }
            }

            Console.WriteLine(newName);
            Console.WriteLine(newUsername);
            Console.WriteLine(newPassword);
        }
    }
}
