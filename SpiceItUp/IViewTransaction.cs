using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Layout of basic implementations for viewing a customers order.
    /// Mainly focusing on getting a transaction history, and then viewing the details of those transactions
    /// </summary>
    public interface IViewTransaction
    {
        static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        static List<string> transList = new List<string>();

        static bool exit = false;

        static int userEntry;

        static void TransactionHistory() { }

        static void DetailedTransaction() { }
    }
}
