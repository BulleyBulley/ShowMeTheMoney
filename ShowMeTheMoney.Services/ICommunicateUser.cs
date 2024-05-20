using ShowMeTheMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowMeTheMoney.Services
{
    public interface ICommunicateUser
    {

        // using interface allows for mocking in tests and open for future implementations, such as a GUI
        void InformUser(string message);
        void DisplayBalance(decimal balance);
        void DisplayTransactionList(List<TransactionLog> transactionLogs);
        string GetUserInput(string prompt);
    }
}
