using ShowMeTheMoney.Models;
using ShowMeTheMoney.Services;

namespace ShowMeTheMoneyTestProject
{
    public class TestTransactionService
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        // Instantiating a Transaction Service should set the balance to 200
        public void InstantiatingSetsBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);

            // Act
            decimal balance = transactionService.GetBalance();

            // Assert
            Assert.That(balance, Is.EqualTo(200m));
        }

        [Test]
        // Depositing 100 should increase the balance to 300
        public void DepositIncreasesBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Deposit deposit = new Deposit { Amount = 100m };

            // Act
            transactionService.Deposit(deposit);
            decimal balance = transactionService.GetBalance();

            // Assert
            Assert.That(balance, Is.EqualTo(300m));
        }

        [Test]
        // Withdrawing 100 should decrease the balance to 100
        public void WithdrawDecreasesBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Withdrawal withdrawal = new Withdrawal { Amount = 100m };

            // Act
            transactionService.Withdraw(withdrawal);
            decimal balance = transactionService.GetBalance();

            // Assert
            Assert.That(balance, Is.EqualTo(100m));
        }

        [Test]
        // Depositing 100 and withdrawing 50 should result in a balance of 250
        public void DepositAndWithdrawGivesCorrectBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Deposit deposit = new Deposit { Amount = 100m };
            Withdrawal withdrawal = new Withdrawal { Amount = 50m };

            // Act
            transactionService.Deposit(deposit);
            transactionService.Withdraw(withdrawal);
            decimal balance = transactionService.GetBalance();

            // Assert
            Assert.That(balance, Is.EqualTo(250m));
        }

        [Test]
        // Depositing 100 and withdrawing 50 should result in a transaction log with 2 entries
        public void DepositAndWithdrawGivesCorrectTransactionLogAmount()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Deposit deposit = new Deposit { Amount = 100m};
            Withdrawal withdrawal = new Withdrawal { Amount = 50m};

            // Act
            transactionService.Deposit(deposit);
            transactionService.Withdraw(withdrawal);
            List<TransactionLog> transactionLog = transactionService.GetTransactionLog();

            // Assert
            Assert.That(transactionLog.Count, Is.EqualTo(2));
        }

        [Test]
        // Depositing 100 and withdrawing 50 should result in a transaction log with the correct entries
        public void DepositAndWithdrawGivesCorrectTransactionLogEntries()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Deposit deposit = new Deposit { Amount = 100m};
            Withdrawal withdrawal = new Withdrawal { Amount = 50m};

            // Act
            transactionService.Deposit(deposit);
            transactionService.Withdraw(withdrawal);
            List<TransactionLog> transactionLog = transactionService.GetTransactionLog();

            // Assert
            Assert.That(transactionLog[0].Amount, Is.EqualTo(100m));
            Assert.That(transactionLog[0].TransactionDescription, Is.EqualTo(TransactionLog.TransactionType.Deposit));
            Assert.That(transactionLog[1].Amount, Is.EqualTo(50m));
            Assert.That(transactionLog[1].TransactionDescription, Is.EqualTo(TransactionLog.TransactionType.Withdrawal));
        }

        [Test]
        // Depositing 100 and withdrawing 50 should result in a transaction log with the correct balances
        public void DepositAndWithdrawGivesCorrectTransactionLogBalances()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Deposit deposit = new Deposit { Amount = 100m };
            Withdrawal withdrawal = new Withdrawal { Amount = 50m };

            // Act
            transactionService.Deposit(deposit);
            transactionService.Withdraw(withdrawal);
            List<TransactionLog> transactionLog = transactionService.GetTransactionLog();

            // Assert
            Assert.That(transactionLog[0].OldBalance, Is.EqualTo(200m));
            Assert.That(transactionLog[0].NewBalance, Is.EqualTo(300m));
            Assert.That(transactionLog[1].OldBalance, Is.EqualTo(300m));
            Assert.That(transactionLog[1].NewBalance, Is.EqualTo(250m));
        }

        [Test]
        //CanWithdraw returns false with insufficient balance
        public void CanWithdrawReturnsFalseWithInsufficientBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Withdrawal withdrawal = new Withdrawal { Amount = 5000m };

            // Act
            bool canWithdraw = transactionService.CanWithdraw(withdrawal.Amount);

            //Assert
            Assert.IsFalse(canWithdraw);
            
        }

        [Test]
        //CanWithdraw returns true with sufficient balance
        public void CanWithdrawReturnsTrueWithSufficientBalance()
        {
            // Arrange
            TransactionService transactionService = new TransactionService(200m);
            Withdrawal withdrawal = new Withdrawal { Amount = 50m };

            // Act
            bool canWithdraw = transactionService.CanWithdraw(withdrawal.Amount);

            //Assert
            Assert.IsTrue(canWithdraw);
        }
    }
}