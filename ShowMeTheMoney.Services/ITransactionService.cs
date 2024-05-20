using ShowMeTheMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowMeTheMoney.Services
{
    public interface ITransactionService
    {
        decimal GetBalance();
        decimal Deposit(Deposit deposit);
        decimal Withdraw(Withdrawal withdrawal);
        bool CanWithdraw(decimal amount);
        List<TransactionLog> GetTransactionLog();
    }
}
