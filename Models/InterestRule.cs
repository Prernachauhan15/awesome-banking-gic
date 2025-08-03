using System;

public class InterestRule
{
    public DateTime Date { get; set; }
    public string RuleId { get; set; }
    public decimal Rate { get; set; }

    public InterestRule(DateTime date, string ruleId, decimal rate)
    {
        Date = date;
        RuleId = ruleId;
        Rate = rate;
    }
}