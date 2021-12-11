using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerAccount
    {
        protected string firstName;
        protected string lastName;

        public CustomerAccount(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}
