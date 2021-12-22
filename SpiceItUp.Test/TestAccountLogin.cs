using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// Check if the account exists
    /// Is this user an employee or not?
    /// </summary>
    public class TestAccountLogin
    {
        /// <summary>
        /// Can we find a valid account based on a username and password
        /// </summary>
        [Fact]
        public void TestEntries_UsernamePassword_ValidAccount()
        {
            //Arrange
            string username = "ManagerKyler";
            string password = "ManagerPassword";
            string firstNameResult = "Kyler";
            string lastNameResult = "Dennis";

            //Act
            AccountLogin.TestEntries(username, password);

            //Assert
            Assert.True(firstNameResult.Equals(AccountLogin.firstName) && lastNameResult.Equals(AccountLogin.lastName));
        }

        /// <summary>
        /// The account we find should be an employee
        /// </summary>
        [Fact]
        public void TestEntries_UsernamePassword_EmployeeTrue()
        {
            //Arrange
            string username = "ManagerKyler";
            string password = "ManagerPassword";
            string isEmployeeResult = "TRUE";

            //Act
            AccountLogin.TestEntries(username, password);

            //Assert
            Assert.True(isEmployeeResult.Equals(AccountLogin.isEmployee));
        }

        /// <summary>
        /// The account we find should not be an employee
        /// </summary>
        [Fact]
        public void TestEntries_UsernamePassword_EmployeeFalse()
        {
            //Arrange
            string username = "Daniel0";
            string password = "Everett0";
            string isEmployeeResult = "FALSE";

            //Act
            AccountLogin.TestEntries(username, password);

            //Assert
            Assert.True(isEmployeeResult.Equals(AccountLogin.isEmployee));
        }
    }
}