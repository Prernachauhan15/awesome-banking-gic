// StatementService.cs
// Handles generation of monthly account statements including interest calculation.

using System;
using System.Collections.Generic;
using System.Linq;



/// <summary>
/// Responsible for printing monthly statements including transactions and interest calculations.
/// </summary>
public class StatementService
{
    private readonly TransactionService _txnService;
    private readonly InterestService _interestService;

    /// <summary>
    /// Constructor to initialize the transaction and interest services.
    /// </summary>
    public StatementService(TransactionService txnService, InterestService interestService)
    {
        _txnService = txnService;
        _interestService = interestService;
    }

    /// <summary>
    /// Prints the monthly statement for a given account.
    /// </summary>
    public void PrintMonthlyStatement(string account, int year, int month)
    {
        // Filter transactions for the target month.
        var transactions = _txnService.GetTransactions(account).Where(t => t.Date.Year == year && t.Date.Month == month).ToList();
        // Calculate opening balance as of the 1st of the month.
        decimal balance = _txnService.GetTransactions(account).Where(t => t.Date <= new DateTime(year, month, 1)).Sum(t => t.Type == "D" || t.Type == "I" ? t.Amount : -t.Amount);

        // Define the period range for interest calculation.
        DateTime start = new(year, month, 1);
        DateTime end = new(year, month, DateTime.DaysInMonth(year, month));

        // Calculate interest for the month.
        var interest = CalculateInterest(account, start, end);

        // Print the account header and transaction table.
        Console.WriteLine($"Account: {account}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");

        // Print each transaction and update the running balance.
        foreach (var t in transactions.OrderBy(t => t.Date).ThenBy(t => t.TransactionId))
        {
            balance += t.Type == "D" ? t.Amount : -t.Amount;
            Console.WriteLine($"| {t.Date:yyyyMMdd} | {t.TransactionId,-10} | {t.Type}    | {t.Amount,6:F2} | {balance,7:F2} |");
        }

        // Add and print the interest earned at the end of the month, if any.
        if (interest > 0)
        {
            balance += interest;
            Console.WriteLine($"| {end:yyyyMMdd} |            | I    | {interest,6:F2} | {balance,7:F2} |");
        }
    }

    /// <summary>
    /// Calculates the interest for a given account over a specific date range.
    /// </summary>
    private decimal CalculateInterest(string account, DateTime start, DateTime end)
    {
        decimal interest = 0;
        DateTime current = start;
        // Calculate opening balance before interest period starts.
        decimal balance = _txnService.GetTransactions(account).Where(t => t.Date <= current).Sum(t => t.Type == "D" || t.Type == "I" ? t.Amount : -t.Amount);
        // Get all transactions in the period sorted by date.
        var transactions = _txnService.GetTransactions(account).Where(t => t.Date >= start && t.Date <= end).OrderBy(t => t.Date).ToList();

        // Iterate over the interest period day-by-day 
        while (current <= end)
        {
            // Get the interest rule applicable on the current day.
            var rule = _interestService.GetRuleByDate(current);
            // Determine the next date when the rule changes within the same month.
            DateTime nextChange = _interestService.GetRules().Where(r => r.Date > current && r.Date <= end).Select(r => r.Date).DefaultIfEmpty(end.AddDays(1)).Min();
            // Determine the end of the interest calculation period chunk.
            DateTime periodEnd = transactions.Where(t => t.Date > current && t.Date < nextChange).Select(t => t.Date.AddDays(-1)).DefaultIfEmpty(nextChange.AddDays(-1)).Min();

            // Calculate the number of days for which the same rule is applied.
            int days = (periodEnd - current).Days + 1;
            // Apply daily interest calculation formula: (balance * rate * days) / 36500
            interest += (balance * (rule?.Rate ?? 0) * days) / 36500m;

            // Update the balance based on transactions in this chunk.
            var dailyTxns = transactions.Where(t => t.Date >= current && t.Date <= periodEnd).ToList();
            foreach (var t in dailyTxns)
                balance += t.Type == "D" ? t.Amount : -t.Amount;

            // Move to the next day after this period.
            current = periodEnd.AddDays(1);
        }

        return Math.Round(interest, 2);
    }
}
