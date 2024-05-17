using ShowMeTheMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowMeTheMoney.Services
{
    public class CommunicateUser
    {
        public static void InformUser(string message)
        {
            Console.WriteLine(message);
        }

        public static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static void DisplayBalance(decimal balance)
        {
            Console.WriteLine($"Your balance is £{balance}");
        }

        public static void DisplayTransactionList(List<TransactionLog> transactions)
        {
            Console.WriteLine("Transaction List:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }
    }
}
