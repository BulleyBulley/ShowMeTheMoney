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
        public void InformUser(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayBalance(decimal balance)
        {
            Console.WriteLine($"Your current balance is: £{balance:F2}");
        }

        public string GetUserInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public void DisplayTransactionList(List<TransactionLog> transactionLogs)
        {
            foreach (TransactionLog log in transactionLogs)
            {
                Console.WriteLine($"Date: {log.TransactionDate}, Amount: £{log.Amount:F2}, Description: {log.TransactionDescription}, Balance: £{log.NewBalance:F2}");
            }
        }
    }


}

