using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// Test if we can't view an invalid transaction
    /// </summary>
    public class TestCustomerOrderHistory
    {
        /// <summary>
        /// We should not be able to pull up a valid user with an invalid user ID
        /// </summary>
        [Fact]
        public void CustomerTransactionHistory_UserID_InvalidUser()
        {
            //Act
            CustomerOrderHistory.CustomerTransactionHistory(0);

            //Assert
            Assert.True(CustomerOrderHistory.transList.Count == 0);
        }
    }
}
