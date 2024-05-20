using ShowMeTheMoney.Services;
using ShowMeTheMoney.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShowMeTheMoney.Models;

namespace ShowMeTheMoneyTestProject
{
    public class TestConsoleApp
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ValidateInput_ValidInput_ReturnsTrue()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput("500");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateInput_EmptyInput_ReturnsFalse()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput("");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateInput_NullInput_ReturnsFalse()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateInput_NonNumericInput_ReturnsFalse()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput("abc");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateInput_ZeroInput_ReturnsFalse()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput("0");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateInput_NegativeInput_ReturnsFalse()
        {
            // Arrange
            var program = new Program(null, null);

            // Act
            bool result = program.ValidateInput("-500");

            // Assert
            Assert.IsFalse(result);
        }



        [Test]
        public void TestInitialDeposit()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.InitialDeposit();

            // Assert
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 1500m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("A further £1500 has been deposited into your account"), Times.Once);
            mockCommunicateUser.Verify(user => user.DisplayBalance(It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void TestInitialWithdrawal_ValidInput()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.Setup(m => m.GetUserInput(It.IsAny<string>())).Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.InitialWithdrawal();

            // Assert
            mockTransactionService.Verify(service => service.Withdraw(It.Is<Withdrawal>(w => w.Amount == 500m)), Times.Once);
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 250m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("Invalid input, please try again"), Times.Never);
        }

        [Test]
        public void TestInitialWithdrawal_InvalidInput()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.SetupSequence(m => m.GetUserInput(It.IsAny<string>()))
                .Returns("abc")
                .Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.InitialWithdrawal();

            // Assert
            mockTransactionService.Verify(service => service.Withdraw(It.Is<Withdrawal>(w => w.Amount == 500m)), Times.Once);
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 250m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("Invalid input, please try again"), Times.Once);
        }

        [Test]
        // test InitialWithdrawal with negative input
        public void TestInitialWithdrawal_NegativeInput()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.SetupSequence(m => m.GetUserInput(It.IsAny<string>()))
                .Returns("-500")
                .Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.InitialWithdrawal();

            // Assert
            mockTransactionService.Verify(service => service.Withdraw(It.Is<Withdrawal>(w => w.Amount == 500m)), Times.Once);
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 250m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("Invalid input, please try again"), Times.Once);
        }

        [Test]
        public void HandleTransaction_ValidDepositInput_CallsProcessDeposit()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.Setup(m => m.GetUserInput(It.IsAny<string>())).Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.HandleTransaction(true, false, false);

            // Assert
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 500m)), Times.Once);
        }

        [Test]
        public void HandleTransaction_ValidWithdrawalInput_CallsProcessWithdrawal()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService.Setup(m => m.CanWithdraw(It.IsAny<decimal>())).Returns(true);
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.Setup(m => m.GetUserInput(It.IsAny<string>())).Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.HandleTransaction(false, false, false);

            // Assert
            mockTransactionService.Verify(service => service.Withdraw(It.Is<Withdrawal>(w => w.Amount == 500m)), Times.Once);
        }

        [Test]
        public void HandleTransaction_InvalidInput_InformsUser()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.Setup(m => m.GetUserInput(It.IsAny<string>())).Returns("abc");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.HandleTransaction(true, false, false);

            // Assert
            mockCommunicateUser.Verify(user => user.InformUser("Invalid input, please try again"), Times.Once);
        }

        [Test]
        public void HandleTransaction_InsufficientFunds_InformsUser()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService.Setup(m => m.CanWithdraw(It.IsAny<decimal>())).Returns(false);
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            mockCommunicateUser.Setup(m => m.GetUserInput(It.IsAny<string>())).Returns("500");
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.HandleTransaction(false, false, false);

            // Assert
            mockCommunicateUser.Verify(user => user.InformUser("Insufficient funds, please try again"), Times.Once);
        }


        [Test]
        public void ProcessDeposit_ValidAmount_CallsDepositAndInformUser()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessDeposit(500m, false, false);

            // Assert
            mockTransactionService.Verify(service => service.Deposit(It.Is<Deposit>(d => d.Amount == 500m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("Depositing £500.00"), Times.Once);
        }

        [Test]
        public void ProcessDeposit_ShowBalance_CallsDisplayBalance()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessDeposit(500m, true, false);

            // Assert
            mockCommunicateUser.Verify(user => user.DisplayBalance(It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void ProcessDeposit_ShowLogs_CallsDisplayLogs()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessDeposit(500m, false, true);

            // Assert
            mockCommunicateUser.Verify(user => user.DisplayTransactionList(It.IsAny<List<TransactionLog>>()), Times.Once);
        }


        [Test]
        public void ProcessWithdrawal_ValidAmount_CallsWithdrawAndInformUser()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessWithdrawal(500m, false, false);

            // Assert
            mockTransactionService.Verify(service => service.Withdraw(It.Is<Withdrawal>(w => w.Amount == 500m)), Times.Once);
            mockCommunicateUser.Verify(user => user.InformUser("Withdrawing £500.00"), Times.Once);
        }

        [Test]
        public void ProcessWithdrawal_ShowBalance_CallsDisplayBalance()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessWithdrawal(500m, true, false);

            // Assert
            mockCommunicateUser.Verify(user => user.DisplayBalance(It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void ProcessWithdrawal_ShowLogs_CallsDisplayLogs()
        {
            // Arrange
            var mockTransactionService = new Mock<ITransactionService>();
            var mockCommunicateUser = new Mock<ICommunicateUser>();
            var program = new Program(mockTransactionService.Object, mockCommunicateUser.Object);

            // Act
            program.ProcessWithdrawal(500m, false, true);

            // Assert
            mockCommunicateUser.Verify(user => user.DisplayTransactionList(It.IsAny<List<TransactionLog>>()), Times.Once);
        }

    }
}
