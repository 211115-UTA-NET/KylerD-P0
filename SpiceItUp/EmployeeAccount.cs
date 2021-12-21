using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// A user directed to this class is flagged as an employee.
    /// Employees are given options for how to proceed.
    /// </summary>
    public class EmployeeAccount
    {
        protected int userID;
        protected string firstName;
        protected string lastName;
        protected double phoneNumber;

        private bool logout = false;

        /// <summary>
        /// Constructor to get the basic information from employee.
        /// This information was pulled from the database when the employee logged in.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        public EmployeeAccount(int userID, string firstName, string lastName, double phoneNumber)
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
                Console.WriteLine($"Welcome, employee {firstName} {lastName[0]}! What would you like to do?");
                Console.WriteLine("1: View orders by customer name");
                Console.WriteLine("2: View orders by store number");
                Console.WriteLine("3: View store inventory");
                Console.WriteLine("4: Lookup customers by name");
                Console.WriteLine("5: Log out");

                int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && userEntry >= 1 && userEntry <= 5)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1: //View order histories by customer name
                        SpiceItUp.EmployeeTransactionByCustomer.SelectACustomer();
                        break;
                    case 2: //View order histories by store 
                        SpiceItUp.EmployeeTransactionByStore.StoreSelection();
                        break;
                    case 3: //View a stores inventory
                        SpiceItUp.LocationInventory.StoreSelection();
                        break;
                    case 4: //Pull a customer's account information by looking up their name
                        SpiceItUp.CustomerLookup.CustomerSearchOptions();
                        break;
                    case 5: //Log out of the employye account and return to the login page
                        Console.WriteLine("Goodbye!");
                        logout = true;
                        break;
                }
            }
        }
    }
}
