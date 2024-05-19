using ShowMeTheMoney.Models;
using ShowMeTheMoney.Services;

namespace ShowMeTheMoney.ConsoleApp
{
    internal class Program
    {
        private TransactionService _transactionService = new TransactionService(200m);
        private CommunicateUser _communicateUser = new CommunicateUser();
        static void Main(string[] args)
        {

            var program = new Program();
            program.RunApp();

            // Task 1: Instantiate a TransactionService with a balance of 200 and inform the user that this has happened

            // Task 2: Deposit 1500 and return the balance in a readable format

            // Task 3: Withdraw an amount determined by the user and return the balance in a readable format

            // Task 4: Deposit half the the amount withdrawn by the user

            // Task 5: Ask the user if they want to continue with the program

            // Task 6: If yes, ask the user if they want to deposit or withdraw, and handle accordingly

            // Task 7: Print the transaction list to the user

        }

        /// <summary>
        /// Run the application
        /// </summary>
        private void RunApp()
        {
            _communicateUser.InformUser("Welcome to Show Me The Money!");

            InitialDeposits();
            InitialWithdrawal();
            MainLoop();
        }

        /// <summary>
        /// Initial deposits
        /// </summary>
        private void InitialDeposits()
        {
            _communicateUser.InformUser("Congratulations!! A not at all suspicious email has given you £200");
            System.Threading.Thread.Sleep(2000);
            _communicateUser.InformUser("And now they're depositing a further £1500 into your account");
            System.Threading.Thread.Sleep(2000);
            _transactionService.Deposit(new Deposit { Amount = 1500m });
            _communicateUser.DisplayBalance(_transactionService.GetBalance());
        }

        /// <summary>
        /// Initial withdrawal and subsequent deposit
        /// </summary>
        private void InitialWithdrawal()
        {
            decimal withdrawalAmount;

            while (true)
            {
                withdrawalAmount = decimal.Parse(_communicateUser.GetUserInput("How much would you like to withdraw?"));

                if (_transactionService.CanWithdraw(withdrawalAmount))
                {
                    break;
                }

                _communicateUser.InformUser("Insufficient funds. Please enter a different amount.");
            }

            _transactionService.Withdraw(new Withdrawal { Amount = withdrawalAmount });
            _communicateUser.DisplayBalance(_transactionService.GetBalance());

            // Limit to 2 decimal places
            decimal depositAmount = decimal.Round(withdrawalAmount / 2, 2);

            _communicateUser.InformUser($"More good luck, you've won a beauty contest that you didn't even enter and they're depositing half your withdrawal, so £{depositAmount} goes back into your account");
            _transactionService.Deposit(new Deposit { Amount = depositAmount });
            _communicateUser.DisplayBalance(_transactionService.GetBalance());
        }


        /// <summary>
        /// main loop of the program
        /// </summary>
        private void MainLoop()
        {
            while (true)
            {
                if (!AskContinue())
                    break;

                HandleUserChoice();
            }
        }

        /// <summary>
        /// Ask the user if they want to continue
        /// </summary>
        /// <returns></returns>
        private bool AskContinue()
        {
            string response = _communicateUser.GetUserInput("Would you like to continue? Please enter 'y' or 'n'");
            // if something other than y or n is entered, ask again
            while (response.ToLower() != "y" && response.ToLower() != "n")
            {
                response = _communicateUser.GetUserInput("Invalid input, please enter 'y' or 'n'");
            }
            return response.ToLower() == "y";
        }

        /// <summary>
        /// Handle the user's choice of deposit, withdraw or see transaction list
        /// </summary>
        private void HandleUserChoice()
        {
            string transactionType = _communicateUser.GetUserInput("Would you like to deposit, withdraw or see transaction list, press 'd', 'w' or 'p'?");
            switch (transactionType.ToLower())
            {
                case "d":
                    decimal depositAmount = decimal.Parse(_communicateUser.GetUserInput("How much would you like to deposit?"));
                    _transactionService.Deposit(new Deposit { Amount = depositAmount });
                    _communicateUser.DisplayBalance(_transactionService.GetBalance());
                    break;
                case "w":
                    decimal withdrawAmount = decimal.Parse(_communicateUser.GetUserInput("How much would you like to withdraw?"));
                    //checking for sufficient funds
                    if (!_transactionService.CanWithdraw(withdrawAmount))
                    {
                        _communicateUser.InformUser("Insufficient funds");
                        return;
                    }
                    _transactionService.Withdraw(new Withdrawal { Amount = withdrawAmount });
                    _communicateUser.DisplayBalance(_transactionService.GetBalance());
                    break;
                case "p":
                    List<TransactionLog> transactionLogs = _transactionService.GetTransactionLog();
                    _communicateUser.DisplayTransactionList(transactionLogs);
                    break;
                default:
                    _communicateUser.InformUser("Invalid input, please try again");
                    break;
            }
        }


    }
}
