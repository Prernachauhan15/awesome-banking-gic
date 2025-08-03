using System;
using Xunit;
public class InterestServiceTests
{
    [Fact]
    public void AddOrUpdateInterestRule_AddsRuleCorrectly()
    {
        var service = new InterestService();
        service.AddOrUpdateInterestRule(new InterestRule(new DateTime(2023, 6, 15), "RULE03", 2.2m));
        var rules = service.GetRules();
        Assert.Single(rules);
        Assert.Equal("RULE03", rules[0].RuleId);
    }

    [Fact]
    public void AddOrUpdateInterestRule_OverridesRuleWithSameDate()
    {
        var service = new InterestService();
        service.AddOrUpdateInterestRule(new InterestRule(new DateTime(2023, 6, 15), "RULE03", 2.2m));
        service.AddOrUpdateInterestRule(new InterestRule(new DateTime(2023, 6, 15), "RULE04", 3.0m));
        var rules = service.GetRules();
        Assert.Single(rules);
        Assert.Equal("RULE04", rules[0].RuleId);
    }

    [Fact]
    public void GetRuleByDate_ReturnsMostRecentRuleBeforeOrOnDate()
    {
        var service = new InterestService();
        service.AddOrUpdateInterestRule(new InterestRule (new DateTime(2025, 1, 1),  "RULE01", 4.0m) );
        service.AddOrUpdateInterestRule(new InterestRule (new DateTime(2025, 3, 1), "RULE02", 5.0m ));

        var rule = service.GetRuleByDate(new DateTime(2025, 2, 1));

        Assert.Equal("RULE01", rule.RuleId);
    }

    [Fact]
    public void GetRuleByDate_NoRules_ReturnsNull()
    {
        var service = new InterestService();
        var result = service.GetRuleByDate(new DateTime(2025, 1, 1));
        Assert.Null(result);
    }

    [Fact]
    public void GetRuleByDate_ReturnsCorrectRule_BeforeDate()
    {
        var service = new InterestService();
        service.AddOrUpdateInterestRule(new InterestRule (new DateTime(2025, 1, 1), "RULE01",  1 ));
        service.AddOrUpdateInterestRule(new InterestRule (new DateTime(2025, 2, 1),"RULE02",  2 ));

        var rule = service.GetRuleByDate(new DateTime(2025, 2, 15));
        Assert.Equal("RULE02", rule.RuleId);
    }
}