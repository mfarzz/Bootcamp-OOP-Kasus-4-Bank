namespace Bank.Class;

public class Transaction
{
    public string Type { get; }
    public double Amount { get; }
    public double BalanceAfter { get; }
    public DateTime TransactionDate { get; }

    public Transaction(string type, double amount, double balanceAfter)
    {
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        TransactionDate = DateTime.Now;
    }

    public void PrintStatement()
    {
        Console.WriteLine(
            $"{TransactionDate:dd/MM/yyyy HH:mm:ss} | " +
            $"{Type,-12} | " +
            $"Amount: {Amount:N2} | " +
            $"Balance: {BalanceAfter:N2}"
        );
    }
}