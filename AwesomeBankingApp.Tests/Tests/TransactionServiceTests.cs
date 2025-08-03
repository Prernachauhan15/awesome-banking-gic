using System;
using Xunit;

public class TransactionServiceTests
{
    [Fact]
    public void AddTransaction_Deposit_AddsTransactionCorrectly()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2023, 6, 1), "AC001", "D", 100);

        var txns = service.GetTransactions("AC001");
        Assert.Single(txns);
        Assert.Equal("D", txns[0].Type);
        Assert.Equal(100, txns[0].Amount);
    }

    [Fact]
    public void AddTransaction_Withdraw_ThrowsWhenInsufficientBalance()
    {
        var service = new TransactionService();
        Assert.Throws<InvalidOperationException>(() =>
            service.AddTransaction(new DateTime(2023, 6, 1), "AC001", "W", 100));
    }

    [Fact]
    public void AddTransaction_Deposit_IncreasesBalance()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 1), "ACC1", "D", 1000);

        Assert.Equal(1000, service.GetBalance("ACC1"));
    }

    [Fact]
    public void AddTransaction_Withdraw_DecreasesBalance()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 1), "ACC1", "D", 1000);
        service.AddTransaction(new DateTime(2025, 1, 2), "ACC1", "W", 400);

        Assert.Equal(600, service.GetBalance("ACC1"));
    }

    [Fact]
    public void AddTransaction_WithdrawMoreThanBalance_Throws()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 1), "ACC1", "D", 300);

        Assert.Throws<InvalidOperationException>(() =>
            service.AddTransaction(new DateTime(2025, 1, 2), "ACC1", "W", 400));
    }

    [Fact]
    public void GetTransactions_ReturnsTransactionsSorted()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 2), "ACC1", "D", 200);
        service.AddTransaction(new DateTime(2025, 1, 1), "ACC1", "D", 100);

        var txns = service.GetTransactions("ACC1");
        Assert.Equal(2, txns.Count);
        Assert.True(txns[0].Date < txns[1].Date);
    }

    [Fact]
    public void Withdraw_EntireBalance_SetsBalanceToZero()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 1), "A1", "D", 100);
        service.AddTransaction(new DateTime(2025, 1, 2), "A1", "W", 100);
        Assert.Equal(0, service.GetBalance("A1"));
    }

    [Fact]
    public void BalanceCalculation_MixedTransactions_IsCorrect()
    {
        var service = new TransactionService();
        service.AddTransaction(new DateTime(2025, 1, 1), "A1", "D", 500);
        service.AddTransaction(new DateTime(2025, 1, 2), "A1", "W", 100);
        service.AddTransaction(new DateTime(2025, 1, 3), "A1", "I", 10);

        Assert.Equal(410, service.GetBalance("A1"));
    }
}
