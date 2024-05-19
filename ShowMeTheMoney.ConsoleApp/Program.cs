using ShowMeTheMoney.Models;
using ShowMeTheMoney.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ShowMeTheMoney.ConsoleApp
{
    internal class Program
    {
        private TransactionService _transactionService;
        private CommunicateUser _communicateUser = new CommunicateUser();
        private int dialogPause = 1000;
        private decimal startingBalance = 200m;

        static void Main(string[] args)
        {
            Program program = new Program();
            try
            {
                program.RunApp();
            }
            catch (Exception e)
            {
                program._communicateUser.InformUser("An error occurred. Please restart the application. Error: " + e);
            }

        }

        /// <summary>
        /// Start the transaction service
        /// </summary>
        private void StartTransactionService()
        {
            _transactionService = new TransactionService(startingBalance);
        }

        /// <summary>
        /// Run the application - entry point
        /// </summary>
        private void RunApp()
        {
            _communicateUser.InformUser("Welcome to Show Me The Money!");

            try
            {
                StartTransactionService();
                _communicateUser.InformUser($"TransactionService started. Your account has been set up with £ {startingBalance:F2}");
            }
            catch (Exception e)
            {
                _communicateUser.InformUser("An error occurred. Please restart the application. Error: " + e);
            }

            InitialDeposits();
            InitialWithdrawal();
            
            MainLoop();
        }

        /// <summary>
        /// Initial deposits adds 1500 to the account
        /// </summary>
        private void InitialDeposits()
        {
            Thread.Sleep(dialogPause);
            HandleDeposit("A further £1500 has been deposited into your account", 1500m, showLogs: false);
            _communicateUser.DisplayBalance(_transactionService.GetBalance());
        }

        /// <summary>
        /// Initial withdrawal
        /// </summary>
        private void InitialWithdrawal()
        {
            HandleWithdrawal(isInitial: true, showLogs: false);
            //check _transactionService is not null
            if (_transactionService != null)
            {
                _communicateUser.DisplayBalance(_transactionService.GetBalance());
            }
            
        }


        /// <summary>
        /// Handle deposit
        /// </summary>
        private void HandleDeposit(string message = null, decimal amount = 0, bool showLogs = false)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _communicateUser.InformUser(message);
                Thread.Sleep(dialogPause);
            }

            if (amount == 0)
            {
                amount = GetValidAmount("How much would you like to deposit?");
            }

            // Deposit the amount
            _transactionService.Deposit(new Deposit { Amount = amount });

            // Show the balance and transaction list
            if (showLogs)
            {
                _communicateUser.DisplayBalance(_transactionService.GetBalance());
                _communicateUser.DisplayTransactionList(_transactionService.GetTransactionLog());
            }
        }

        /// <summary>
        /// Handle withdrawal
        /// </summary>
        private void HandleWithdrawal(string message = null, bool showLogs = false, bool isInitial = false)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _communicateUser.InformUser(message);
            }

            decimal amount;
            while (true)
            {
                amount = GetValidAmount("How much would you like to withdraw?");
                if (_transactionService.CanWithdraw(amount))
                {
                    _communicateUser.InformUser($"Withdrawing £{amount:F2}");
                    _transactionService.Withdraw(new Withdrawal { Amount = amount });
                    break;
                }

                _communicateUser.InformUser("Insufficient funds. Please enter a different amount.");
            }

            _communicateUser.DisplayBalance(_transactionService.GetBalance());

            if (isInitial)
            {
                DepositHalfWithdrawal(amount);
            }
            else if (showLogs)
            {
                _communicateUser.DisplayTransactionList(_transactionService.GetTransactionLog());
            }
        }


        /// <summary>
        /// Deposit half of the withdrawal amount
        /// </summary>
        /// <param name="withdrawalAmount"></param>
        private void DepositHalfWithdrawal(decimal withdrawalAmount)
        {
            Thread.Sleep(dialogPause);

            decimal depositAmount = withdrawalAmount / 2;

            HandleDeposit($"More good luck, you've won a beauty contest that you didn't even enter and they're depositing half your withdrawal, so £{depositAmount:F2} goes back into your account",amount: depositAmount, showLogs: false);
        }

        /// <summary>
        /// Main loop of the program
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
                    HandleDeposit(showLogs: true);
                    break;
                case "w":
                    HandleWithdrawal(showLogs: true);
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

        /// <summary>
        /// Get a valid decimal amount from the user
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private decimal GetValidAmount(string prompt)
        {
            decimal amount;
            while (true)
            {
                string input = _communicateUser.GetUserInput(prompt);
                if (!string.IsNullOrWhiteSpace(input) && decimal.TryParse(input, out amount) && amount > 0)
                {
                    return amount;
                }
                _communicateUser.InformUser("Invalid amount. Please enter a valid decimal number greater than 0.");
            }
        }
    }
}
