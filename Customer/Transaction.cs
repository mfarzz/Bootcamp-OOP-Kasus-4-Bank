namespace Bank.Class;

public class Transaction
{
    public int Amount { get; set; }
    public int BalanceAfter { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }

    public Transaction(int amount, int balanceAfter, DateTime date, string type)
    {
        Amount = amount;
        BalanceAfter = balanceAfter;
        Date = date;
        Type = type;
    }

    public void PrintTransaction()
    {
        Console.WriteLine($"{Type} | {Amount:N} | {BalanceAfter:N} | {Date}");
    }
}