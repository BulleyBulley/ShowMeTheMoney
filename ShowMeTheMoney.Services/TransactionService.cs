﻿using ShowMeTheMoney.Models;


namespace ShowMeTheMoney.Services
{
    public class TransactionService : ITransactionService
    {
        private decimal _balance = 0m;
        private List<TransactionLog> _transactionLog = new List<TransactionLog>();

        public TransactionService(decimal initialBalance)
        {
            _balance = initialBalance;
            TransactionLog transactionLog = new TransactionLog(initialBalance, TransactionLog.TransactionType.Setup, 0, initialBalance);

            AddTransactionToList(transactionLog);
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
            TransactionLog transactionLog = new TransactionLog(deposit.Amount, TransactionLog.TransactionType.Deposit, oldBalance, newBalance);

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
            TransactionLog transactionLog = new TransactionLog(withdrawal.Amount, TransactionLog.TransactionType.Withdrawal, oldBalance, newBalance);

            // Add the transaction to the list
            AddTransactionToList(transactionLog);

            return newBalance;
        }

        /// <summary>
        /// Checks if the amount can be withdrawn
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool CanWithdraw(decimal amount)
        {
            return _balance >= amount;
        }

        /// <summary>
        /// Adds a transaction to the list
        /// </summary>
        /// <param name="transactionLog"></param>
        private void AddTransactionToList(TransactionLog transactionLog)
        {

            // TODO: Add the transaction to the log
            // BONUS: Can you use the class to reduce the parameters?

            _transactionLog.Add(transactionLog);
        }

        /// <summary>
        /// returns the transaction log
        /// </summary>
        /// <returns></returns>
        public List<TransactionLog> GetTransactionLog()
        {
            return _transactionLog;
        }
    }
}
