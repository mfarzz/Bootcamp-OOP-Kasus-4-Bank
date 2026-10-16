namespace PracticeBank.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(int balance, int amount, string? detail = null)
        : base(BuildMessage(balance, amount, detail))
    {
        Balance = balance;
        Amount = amount;
        Detail = detail;
    }

    public int Balance { get; }
    public int Amount { get; }
    public string? Detail { get; }

    private static string BuildMessage(int balance, int amount, string? detail)
    {
        var message = $"Saldo tidak mencukupi. Saldo: {Utils.FormatRupiah(balance)}, " +
                      $"jumlah tarik: {Utils.FormatRupiah(amount)}.";
        return detail == null ? message : $"{message} {detail}";
    }
}