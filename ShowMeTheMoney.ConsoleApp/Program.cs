using ShowMeTheMoney.Models;
using ShowMeTheMoney.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ShowMeTheMoney.ConsoleApp
{
    public class Program
    {
        private ITransactionService _transactionService;
        private ICommunicateUser _communicateUser;
        private static decimal startingBalance = 200m;

        /// <summary>
        /// Program constructor
        /// </summary>
        /// <param name="transactionService"></param>
        /// <param name="communicateUser"></param>
        public Program(ITransactionService transactionService, ICommunicateUser communicateUser)
        {
            _transactionService = transactionService;
            _communicateUser = communicateUser;
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //start the transation service with the opening balance
            ITransactionService transactionService = new TransactionService(startingBalance);
            ICommunicateUser communicateUser = new CommunicateUser();

            Program program = new Program(transactionService, communicateUser);

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
        /// Run the application 
        /// </summary>
        private void RunApp()
        {
            _communicateUser.InformUser("Welcome to Show Me The Money");

            try
            {
                _communicateUser.InformUser($"TransactionService started. Your account has been set up with £{startingBalance:F2}");

                InitialDeposit();
                InitialWithdrawal();
                MainLoop();
            }
            catch (Exception e)
            {
                _communicateUser.InformUser("An error occurred. Please restart the application. Error: " + e.Message);
            }
        }


        /// <summary>
        /// Initial deposits adds 1500 to the account
        /// </summary>
        public void InitialDeposit()
        {
            _transactionService.Deposit(new Deposit { Amount = 1500m });
            _communicateUser.InformUser("A further £1500 has been deposited into your account");
            _communicateUser.DisplayBalance(_transactionService.GetBalance());
        }

        /// <summary>
        /// Initial withdrawal
        /// </summary>
        public void InitialWithdrawal()
        {
            string input;
            while (true)
            {
                input = _communicateUser.GetUserInput("How much would you like to withdraw?");
                if (ValidateInput(input))
                {
                    break;
                }
                else
                {
                    _communicateUser.InformUser("Invalid input, please try again");
                }
            }

            decimal amount = decimal.Parse(input);
            ProcessWithdrawal(amount, true, false);
            DepositHalfWithdrawal(amount);
        }

        /// <summary>
        /// Deposit half of the withdrawal amount
        /// </summary>
        /// <param name="withdrawalAmount"></param>
        private void DepositHalfWithdrawal(decimal withdrawalAmount)
        {
            decimal depositAmount = withdrawalAmount / 2;

            _communicateUser.InformUser($"More good luck, a banking error means you get half your withdrawal back, so £{depositAmount:F2} is returned to your account");
            ProcessDeposit(depositAmount, true, false);
        }


        /// <summary>
        /// Handle deposit or withdrawal transactions, show balance and logs
        /// </summary>
        public void HandleTransaction(bool isDeposit, bool showBalance, bool showLogs)
        {
            string input = _communicateUser.GetUserInput($"How much would you like to {(isDeposit ? "deposit" : "withdraw")}?");
            if (ValidateInput(input))
            {
                decimal amount = decimal.Parse(input);
                if (isDeposit)
                {
                    ProcessDeposit(amount, showBalance, showLogs);
                }
                else
                {
                    //need to check if the withdrawal amount is less than the balance
                    if (_transactionService.CanWithdraw(amount))
                    {
                        ProcessWithdrawal(amount, showBalance, showLogs);
                    }
                    else
                    {
                        _communicateUser.InformUser("Insufficient funds, please try again");
                    }
                }
            }
            else
            {
                _communicateUser.InformUser("Invalid input, please try again");
            }
        }


        /// <summary>
        /// Process a deposit transaction, we could combine this with ProcessWithdrawal but leaves them open for future changes
        /// </summary>  
        public void ProcessDeposit(decimal amount, bool showBalance, bool showLogs)
        {
            _transactionService.Deposit(new Deposit { Amount = amount });
            _communicateUser.InformUser($"Depositing £{amount:F2}");

            if (showBalance)
            {
                DisplayBalance();
            }
            if (showLogs)
            {
                DisplayLogs();
            }

        }

        public void ProcessWithdrawal(decimal amount, bool showBalance, bool showLogs)
        {
            _transactionService.Withdraw(new Withdrawal { Amount = amount });
            _communicateUser.InformUser($"Withdrawing £{amount:F2}");

            if (showBalance)
            {
                DisplayBalance();
            }
            if (showLogs)
            {
                DisplayLogs();
            }

        }


        /// <summary>
        /// Main loop of the program
        /// </summary>
        public void MainLoop()
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
                    //deposit
                    HandleTransaction(true, true, true);
                    break;
                case "w":
                    //withdraw
                    HandleTransaction(false, true, true);
                    break;
                case "p":
                    DisplayLogs();
                    break;
                default:
                    _communicateUser.InformUser("Invalid input, please try again");
                    break;
            }
        }

        /// <summary>
        /// Validate the user input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ValidateInput(string input)
        {
            //check if the input is not empty, a valid decimal and greater than 0
            return !string.IsNullOrEmpty(input) && decimal.TryParse(input, out decimal amount) && amount > 0;
        }

        /// <summary>
        /// Print the transaction logs to the console
        /// </summary>
        private void DisplayLogs()
        {
            List<TransactionLog> transactionLogs = _transactionService.GetTransactionLog();
            _communicateUser.DisplayTransactionList(transactionLogs);

        }

        /// <summary>
        /// Print the current balance to the console
        /// </summary>
        private void DisplayBalance()
        {
            _communicateUser.DisplayBalance(_transactionService.GetBalance());
        }
    }
}
