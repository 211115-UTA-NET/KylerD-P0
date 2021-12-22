namespace SpiceItUp
{
    /// <summary>
    /// Starting basic class with starting options.
    /// </summary>
    public class Program
    {
        private static bool logout = false;

        public static string? mySelection;
        public static int userEntry;
        public static bool validEntry;

        /// <summary>
        /// Begin program.
        /// Give user options to create account or sign in.
        /// Based on option entered by user, new class is run
        /// </summary>
        public static void Main()
        {
            while (logout == false)
            {
                Console.WriteLine("Welcome to Spice It Up!");
                Console.WriteLine("Lets get started!");
                Console.WriteLine("1: Create New Account");
                Console.WriteLine("2: Existing Account Login");
                Console.WriteLine("3: Exit");

                //int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    mySelection = Console.ReadLine();
                    validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && userEntry >= 1 && userEntry <= 3)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1:
                        SpiceItUp.NewAccount.CreateAnAccount(); //Create an account
                        break;
                    case 2:
                        SpiceItUp.AccountLogin.LoginManager(); //Log into an account
                        break;
                    case 3: //Stop program by finishing main method
                        Console.WriteLine("Thank you for shopping with us! Have a good day!");
                        logout = true;
                        break;
                }
            }
        }
    }
}