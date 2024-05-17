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
            decimal oldBalance = _balance;
            _balance += deposit.Amount;
            decimal newBalance = _balance;

            // Create a transaction object
            TransactionLog transactionLog = new TransactionLog(deposit.Amount, "Deposit", oldBalance, newBalance);

            // Add the transaction to the list
            AddTransactionToList(transactionLog);

            return newBalance;
        }

        /// <summary>
        /// Subtracts from the balance and returns the new balance
        /// </summary>
        /// <param name="withdrawal"></param>
        /// <returns></returns>
        public decimal Withdraw(Withdrawal withdrawal)
        {
            decimal oldBalance = _balance;
            _balance -= withdrawal.Amount;
            decimal newBalance = _balance;

            // Create a transaction object
            TransactionLog transactionLog = new TransactionLog(withdrawal.Amount, "Withdrawal", oldBalance, newBalance);

            // Add the transaction to the list
            AddTransactionToList(transactionLog);

            return newBalance;
        }

        private void AddTransactionToList(TransactionLog transactionLog)
        {

            // TODO: Add the transaction to the log
            // BONUS: Can you use the class to reduce the parameters?

            _transactionLog.Add(transactionLog);
        }

        public List<TransactionLog> GetTransactionLog()
        {
            return _transactionLog;
        }
    }
}
