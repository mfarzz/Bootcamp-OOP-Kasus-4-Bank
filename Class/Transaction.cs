namespace Bank.Class;

public class Transaction
{
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime DateTime { get; set; }

    public Transaction(string type, decimal amount, decimal balanceAfter)
    {
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        DateTime = DateTime.Now;
    }

    public void PrintStatement()
    {
        Console.WriteLine(
            $"{DateTime:dd/MM/yyyy HH:mm:ss} | " +
            $"{Type,-12} | " +
            $"Amount: {Amount:N2} | " +
            $"Balance: {BalanceAfter:N2}"
        );
    }
}