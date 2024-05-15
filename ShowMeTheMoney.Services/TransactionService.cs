using ShowMeTheMoney.Models;

namespace ShowMeTheMoney.Services
{
    public class TransactionService
    {
        private decimal _balance = 0m;
        private List<TransactionLog> _transactionLog = new List<TransactionLog>();

        public TransactionService(decimal initialBalance)
        {
            _balance = initialBalance;
        }

        /// <summary>
        /// Returns the current balance
        /// </summary>
        /// <returns></returns>
        public decimal GetBalance()
        {
            return _balance;
        }

        /// <summary>
        /// Adds to the balance and returns the new balance
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        public decimal Deposit(Deposit deposit)
        {
            // TODO: Implement updating the balance and returning it. Add the transaction to the log.
        }

        /// <summary>
        /// Subtracts from the balance and returns the new balance
        /// </summary>
        /// <param name="withdrawal"></param>
        /// <returns></returns>
        public decimal Withdraw(Withdrawal withdrawal)
        {
            // TODO: Implement updating the balance and returning it. Add the transaction to the log.
        }

        private void AddTransactionToList(decimal amount, string description, decimal oldBalance, decimal newBalance)
        {
            // TODO: Add the transaction to the log
            // BONUS: Can you use the class to limit this method to 2 parameters?
        }
    }
}
