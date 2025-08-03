
public class Transaction
{
    public DateTime Date { get; set; }
    public string Account { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = "";
}
