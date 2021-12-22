using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// We should not be able to view transaction details if an incorrect store is entered
    /// </summary>
    public class EmployeeTransactionView
    {
        /// <summary>
        /// If we select an incorrect store, we can't pull records
        /// </summary>
        [Fact]
        public void StoreTransactionHistory_UserID_InvalidEntry()
        {
            //Act
            EmployeeTransactionByStore.TransactionHistory(100);

            //Assert
            Assert.False(EmployeeTransactionByStore.transList.Count == 0);
        }

        /// <summary>
        /// If we select an incorrect store, we can't pull records
        /// </summary>
        [Fact]
        public void CustomerNameTransactionHistory_UserID_InvalidEntry()
        {
            //Act
            EmployeeTransactionByCustomer.TransactionHistory(100);

            //Assert
            Assert.False(EmployeeTransactionByCustomer.transList.Count == 0);
        }
    }
}
