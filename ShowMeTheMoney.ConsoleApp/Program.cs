using ShowMeTheMoney.Models;
using ShowMeTheMoney.Services;

namespace ShowMeTheMoney.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitialiseApp();

            // Task 1: Instantiate a TransactionService with a balance of 200 and inform the user that this has happened

            // Task 2: Deposit 1500 and return the balance in a readable format

            // Task 3: Withdraw an amount determined by the user and return the balance in a readable format

            // Task 4: Deposit half the the amount withdrawn by the user

            // Task 5: Ask the user if they want to continue with the program

            // Task 6: If yes, ask the user if they want to deposit or withdraw, and handle accordingly

            // Task 7: Print the transaction list to the user

            


        }

        private static void InitialiseApp()
        {
            CommunicateUser.InformUser("Welcome to Show Me The Money!");
            TransactionService transactionService = new TransactionService(200m);
            CommunicateUser.InformUser("Congratulations!! A not at all suspicious email has given you £200");
            CommunicateUser.InformUser("And now they're depositing a further £1500 into your account");
            transactionService.Deposit(new Deposit { Amount = 1500m });
            decimal balance = transactionService.GetBalance();
            CommunicateUser.DisplayBalance(balance);
            decimal withdrawalAmount = decimal.Parse(CommunicateUser.GetUserInput("How much would you like to withdraw?"));
            transactionService.Withdraw(new Withdrawal { Amount = withdrawalAmount });
            CommunicateUser.DisplayBalance(transactionService.GetBalance());
            CommunicateUser.InformUser($"More good luck, you've won a beauty contest that you didn't even enter and they're depositing half your withdrawal, so £{ withdrawalAmount / 2 } goes back into your account");
            transactionService.Deposit(new Deposit { Amount = withdrawalAmount / 2 });
            CommunicateUser.DisplayBalance(transactionService.GetBalance());

            CommunicateUser.InformUser("Would you like to continue?");
            string response = CommunicateUser.GetUserInput("Please enter 'y' or 'n'");
            if (response == "y")
            {
                string transactionType = CommunicateUser.GetUserInput("Would you like to deposit, withdraw or see transation list, press 'd', 'w' or 'p'?");
                switch (transactionType)
                {
                    case "d":
                        decimal depositAmount = decimal.Parse(CommunicateUser.GetUserInput("How much would you like to deposit?"));
                        transactionService.Deposit(new Deposit { Amount = depositAmount });
                        CommunicateUser.DisplayBalance(transactionService.GetBalance());
                        break;
                    case "w":
                        decimal withdrawAmount = decimal.Parse(CommunicateUser.GetUserInput("How much would you like to withdraw?"));
                        transactionService.Withdraw(new Withdrawal { Amount = withdrawAmount });
                        CommunicateUser.DisplayBalance(transactionService.GetBalance());
                        break;
                    case "p":
                        List<TransactionLog> transactionLogs = transactionService.GetTransactionLog();
                        CommunicateUser.DisplayTransactionList(transactionLogs);
                        break;
                    default:
                        CommunicateUser.InformUser("Invalid input, please try again");
                        break;
                }
            }

        }


    }
}
