﻿namespace Project0
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to Spice It Up!");
            Console.WriteLine("Lets get started!");
            Console.WriteLine("1: Create New Account");
            Console.WriteLine("2: Existing Account Login");
            Console.WriteLine("3: Employee Login");
            Console.WriteLine("4: Exit");

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
                    SpiceItUp.NewAccount.CreateAnAccount();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
    }
}