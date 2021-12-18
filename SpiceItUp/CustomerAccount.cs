using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerAccount
    {
        protected int userID;
        protected string firstName;
        protected string lastName;
        protected double phoneNumber;

        public CustomerAccount(int userID, string firstName, string lastName, double phoneNumber)
        {
            this.userID = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
        }

        public void CustomerOptions()
        {
            Console.WriteLine($"Welcome, {firstName} {lastName[0]}! What would you like to do?");
        }
    }
}
