using System;
using Xunit;
public class StatementServiceTests
{
    [Fact]
    public void PrintMonthlyStatement_NoTransactions_NoException()
    {
        var txnService = new TransactionService();
        var interestService = new InterestService();
        var statementService = new StatementService(txnService, interestService);

        var ex = Record.Exception(() =>
            statementService.PrintMonthlyStatement("AC001", 2023, 6));

        Assert.Null(ex); // Should not throw error even with no transactions
    }

    [Fact]
    public void PrintMonthlyStatement_PrintsInterestAtEnd()
    {
        var txnService = new TransactionService();
        var interestService = new InterestService();
        var statement = new StatementService(txnService, interestService);

        txnService.AddTransaction(new DateTime(2025, 5, 1), "A1", "D", 1000);
        interestService.AddOrUpdateInterestRule(new InterestRule(new DateTime(2025, 5, 1), "IR1", 5)); //5% ANNUAL

       var originalConsoleOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            statement.PrintMonthlyStatement("A1", 2025, 5); // May
            var output = sw.ToString();

            Assert.Contains("I", output);             // Checks interest transaction
            Assert.Contains("20250531", output);      // End of month
            Assert.Contains("1000.00", output);       // Deposit exists
        }
        finally
        {
            Console.SetOut(originalConsoleOut); // always restore
        }
    }


}