using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages transactions per account, including deposits, withdrawals, and interest.
/// </summary>
public class TransactionService
{
    private readonly Dictionary<string, List<Transaction>> _accounts = new();
    private readonly Dictionary<string, int> _txnCounters = new();

    /// <summary>
    /// Adds a new transaction for a given account.
    /// </summary>
    public void AddTransaction(DateTime date, string account, string type, decimal amount)
    {
        // Initialize account's transaction list if not already present.
        if (!_accounts.ContainsKey(account))
            _accounts[account] = new List<Transaction>();

        // Validate balance for withdrawal transactions.
        var balance = GetBalance(account);
        if (type == "W" && balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        // Initialize and increment daily transaction counter to generate unique ID.
        string dateKey = date.ToString("yyyyMMdd");
        if (!_txnCounters.ContainsKey(dateKey))
            _txnCounters[dateKey] = 0;
        _txnCounters[dateKey]++;

        // Generate transaction ID using date and counter.
        var txnId = $"{date:yyyyMMdd}-{_txnCounters[dateKey]:D2}"; ;

        // Create and add the transaction to the account.
        _accounts[account].Add(new Transaction
        {
            Date = date,
            Account = account,
            Type = type,
            Amount = amount,
            TransactionId = txnId
        });
    }

    /// <summary>
    /// Returns all transactions for a given account, sorted by date and transaction ID.
    /// </summary>
    public List<Transaction> GetTransactions(string account)
    {
        return _accounts.ContainsKey(account) ? _accounts[account].OrderBy(t => t.Date).ThenBy(t => t.TransactionId).ToList() : new List<Transaction>();
    }

    /// <summary>
    /// Calculates the current balance for an account by summing all transaction amounts.
    /// Deposits and interest are positive, withdrawals are negative.
    /// </summary>
    public decimal GetBalance(string account)
    {
        return _accounts.ContainsKey(account)
            ? _accounts[account].Sum(t => t.Type == "D" || t.Type == "I" ? t.Amount : -t.Amount)
            : 0;
    }

    /// <summary>
    /// Prints all transactions for an account in tabular format.
    /// </summary>
    public void PrintAccountStatement(string account)
    {
        var txns = GetTransactions(account);
        Console.WriteLine($"Account: {account}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
        foreach (var t in txns)
        {
            Console.WriteLine($"| {t.Date:yyyyMMdd} | {t.TransactionId,-10} | {t.Type}    | {t.Amount,6:F2} |");
        }
    }
}