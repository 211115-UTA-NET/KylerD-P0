using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// Test customer lookup options based on name entered and name type
    /// </summary>
    public class SpiceItUpTest
    {
        /// <summary>
        /// First name entered is a valid customer
        /// </summary>
        [Fact]
        public void CustmerLookup_FirstName_ValidCustomer()
        {
            //Arrange
            string firstName = "Daniel";
            CustomerLookup.firstName = firstName;

            //Act
            CustomerLookup.SearchByFirstName();

            //Assert
            Assert.True(CustomerLookup.test >= 0);
        }

        /// <summary>
        /// First name entered is an invalid customer
        /// </summary>
        [Fact]
        public void CustmerLookup_FirstName_InValidCustomer()
        {
            //Arrange
            string firstName = "Greg";
            CustomerLookup.firstName = firstName;

            //Act
            CustomerLookup.SearchByFirstName();

            //Assert
            Assert.False(CustomerLookup.test >= 0);
        }

        /// <summary>
        /// Last name entered is a valid customer
        /// </summary>
        [Fact]
        public void CustmerLookup_LastName_ValidCustomer()
        {
            //Arrange
            string lastName = "Wooten";
            CustomerLookup.lastName = lastName;

            //Act
            CustomerLookup.SearchByLastName();

            //Assert
            Assert.True(CustomerLookup.test >= 0);
        }

        /// <summary>
        /// Last name entered is an invalid customer
        /// </summary>
        [Fact]
        public void CustmerLookup_LastName_InValidCustomer()
        {
            //Arrange
            string lastName = "Failue";
            CustomerLookup.lastName = lastName;

            //Act
            CustomerLookup.SearchByLastName();

            //Assert
            Assert.False(CustomerLookup.test >= 0);
        }
    }
}
