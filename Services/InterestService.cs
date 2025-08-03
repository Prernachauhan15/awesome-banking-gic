using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Service for managing a list of interest rules over time.
/// </summary>
public class InterestService
{
    private readonly List<InterestRule> _rules = new();

    /// <summary>
    /// Adds a new interest rule or updates an existing one for the same date.
    /// Ensures the list is sorted by date after the update.
    /// </summary>
    public void AddOrUpdateInterestRule(InterestRule rule)
    {
        // To remove existing rule with the same date (if any).
        _rules.RemoveAll(r => r.Date == rule.Date);
        // Add the new or updated rule.
        _rules.Add(rule);
        // Sort rules by date in ascending order.
        _rules.Sort((a, b) => a.Date.CompareTo(b.Date)); 
    }

    /// <summary>
    /// Prints all interest rules in a tabular format to the console.
    /// </summary>
    public void PrintInterestRules()
    {
        Console.WriteLine("Interest rules:");
        Console.WriteLine("| Date     | RuleId | Rate (%) |");
        // Print each rule in ascending order by date.
        foreach (var r in _rules.OrderBy(r => r.Date))
        {
            Console.WriteLine($"| {r.Date:yyyyMMdd} | {r.RuleId,-6} | {r.Rate,8:F2} |");
        }
    }

    /// <summary>
    /// Returns the full list of interest rules.
    /// </summary>
    public List<InterestRule> GetRules() => _rules;

    /// <summary>
    /// Gets the most recent interest rule that is applicable for the specified date.
    /// </summary>
    public InterestRule GetRuleByDate(DateTime date)
    {
        // Find the most recent rule where the rule date is less than or equal to the given date.
        return _rules.Where(r => r.Date <= date).OrderByDescending(r => r.Date).FirstOrDefault();
    }
}