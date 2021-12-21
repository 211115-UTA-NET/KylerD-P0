using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// A user directed to this class is flagged as a customer.
    /// Customers are given options for how to proceed.
    /// </summary>
    public class CustomerAccount
    {
        protected int userID;
        protected string firstName;
        protected string lastName;
        protected double phoneNumber;

        private bool logout = false;

        /// <summary>
        /// Constructor to get the basic information from customer.
        /// This information was pulled from the database when the customer logged in.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        public CustomerAccount(int userID, string firstName, string lastName, double phoneNumber)
        {
            this.userID = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
        }

        /// <summary>
        /// Customers are given different options for how to proceed.
        /// </summary>
        public void UserOptions()
        {
            while (logout == false)
            {
                Console.WriteLine($"Welcome, {firstName} {lastName[0]}! What would you like to do?");
                Console.WriteLine("1: Start a new order");
                Console.WriteLine("2: View order history");
                Console.WriteLine("3: View store inventory");
                Console.WriteLine("4: Log out");

                int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && userEntry >= 1 && userEntry <= 4)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1:
                        SpiceItUp.CustomerOrder.StoreSelection(userID); //Start a new order
                        break;
                    case 2:
                        SpiceItUp.CustomerOrderHistory.CustomerTransactionHistory(userID); //View order history
                        break;
                    case 3:
                        SpiceItUp.LocationInventory.StoreSelection(); //View store inventory
                        break;
                    case 4: //Log out of account
                        Console.WriteLine("Goodbye!");
                        logout = true;
                        break;
                }
            }
        }
    }
}
