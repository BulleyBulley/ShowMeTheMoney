namespace ShowMeTheMoney.Models
{
    public class TransactionLog
    {
        public decimal Amount { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public TransactionType TransactionDescription { get; set; }
        public DateTime TransactionDate { get; set; } // TODO: Make this the time that the transaction is added to the balance

        public TransactionLog(decimal amount, TransactionType transactionDescription, decimal oldBalance, decimal newBalance)
        {
            Amount = amount;
            TransactionDescription = transactionDescription;
            OldBalance = oldBalance;
            NewBalance = newBalance;
            TransactionDate = DateTime.Now;
        }

        public enum TransactionType
        {   
            Setup,
            Deposit,
            Withdrawal
        }
    }
}
