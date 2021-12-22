using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// Test our main program screen
    /// </summary>
    public class TestProgram
    {
        /// <summary>
        /// Test the different entries a customer can enter
        /// </summary>
        /// <param name="selection"></param>
        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        public void Main_MySelection_ValidEntry(string selection)
        {
            //Arrange
            Program.mySelection = selection;

            //Act
            Program.Main();

            //Assert
            if (Program.userEntry >= 1 && Program.userEntry <= 3)
                Assert.True(Program.validEntry);
            else
                Assert.False(Program.validEntry);
        }
    }
}
