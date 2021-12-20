using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerAccount
    {
        public int userID;
        protected string firstName;
        protected string lastName;
        protected double phoneNumber;

        private bool logout = false;

        public CustomerAccount(int userID, string firstName, string lastName, double phoneNumber)
        {
            this.userID = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CustomerAccount()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

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

                //Console.WriteLine(userEntry);
                switch (userEntry)
                {
                    case 1:
                        SpiceItUp.CustomerOrder.StoreSelection(userID);
                        break;
                    case 2:
                        SpiceItUp.CustomerOrderHistory.CustomerTransactionHistory(userID);
                        break;
                    case 3:
                        SpiceItUp.LocationInventory.StoreSelection();
                        break;
                    case 4:
                        Console.WriteLine("Goodbye!");
                        logout = true;
                        break;
                }
            }
        }
    }
}
