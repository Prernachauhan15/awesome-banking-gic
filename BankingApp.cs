using System;
using System.Collections.Generic;
using System.Globalization;


/// <summary>
/// Console-based banking application that supports inputting transactions,
/// defining interest rules, and printing monthly account statements.
/// </summary>
public class BankingApp
{
    // Services for managing transactions, interest rules, and statements.
    private readonly TransactionService _transactionService = new();
    private readonly InterestService _interestService = new();
    private readonly StatementService _statementService;

    /// <summary>
    /// Initializes the banking app and injects required services into the statement service.
    /// </summary>
    public BankingApp()
    {
        _statementService = new StatementService(_transactionService, _interestService);
    }

    /// <summary>
    /// Runs the main application loop that prompts the user for actions.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            // Display the main menu.
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            Console.WriteLine("[T] Input transactions");
            Console.WriteLine("[I] Define interest rules");
            Console.WriteLine("[P] Print statement");
            Console.WriteLine("[Q] Quit");
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToUpper();

            // Handle user input by calling appropriate methods.
            switch (input)
            {
                case "T":
                    InputTransactions();
                    break;
                case "I":
                    DefineInterestRules();
                    break;
                case "P":
                    PrintStatement();
                    break;
                case "Q":
                    Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                    return;
            }
        }
    }

    /// <summary>
    /// Accepts and processes user input for new transactions.
    /// </summary>
    private void InputTransactions()
    {
        Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format\n(or enter blank to go back to main menu):");
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) break;
            var parts = line.Split();

            // Validate input format and data.
            if (parts.Length != 4 || !DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime date)
                || !(parts[2].ToUpper() == "D" || parts[2].ToUpper() == "W")
                || !decimal.TryParse(parts[3], out decimal amount) || amount <= 0 || Math.Round(amount, 2) != amount)
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            try
            {
                // Add the transaction and print updated account statement.
                _transactionService.AddTransaction(date, parts[1], parts[2].ToUpper(), amount);
                _transactionService.PrintAccountStatement(parts[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Accepts and processes user input for defining or updating interest rules.
    /// </summary>
    private void DefineInterestRules()
    {
        Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format\n(or enter blank to go back to main menu):");
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) break;
            var parts = line.Split();
            // Validate rule format and data.
            if (parts.Length != 3 || !DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime date)
                || !decimal.TryParse(parts[2], out decimal rate) || rate <= 0 || rate >= 100)
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            // Add or update interest rule and display the list.
            _interestService.AddOrUpdateInterestRule(new InterestRule(date, parts[1], rate));
            _interestService.PrintInterestRules();
        }
    }

    /// <summary>
    /// Accepts user input to print a monthly statement for a specific account.
    /// </summary>
    private void PrintStatement()
    {
        Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\n(or enter blank to go back to main menu):");
        Console.Write("> ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return;
        var parts = input.Split();

        // Validate input format and parse year/month.
        if (parts.Length != 2 || parts[1].Length != 6 || !int.TryParse(parts[1].Substring(0, 4), out int year) || !int.TryParse(parts[1].Substring(4, 2), out int month))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        // Print the monthly statement for the given account and month.
        _statementService.PrintMonthlyStatement(parts[0], year, month);
    }
}
