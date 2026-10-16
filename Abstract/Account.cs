namespace PracticeBank.Abstract;

public abstract class Account(string accountNumber, string customerName, int balance)
{
    private readonly List<string> _history = new();

    public string AccountNumber { get; } = accountNumber;
    public string OwnerName { get; } = customerName;
    public int Balance { get; protected set; } = balance;
    public IReadOnlyList<string> History => _history;

    public void Deposit(int amount)
    {
        ValidateAmount(amount);
        Balance += amount;
        RecordHistory($"Deposit : {Utils.FormatRupiah(amount)}");
    }

    public void Deposit(int amount, string note)
    {
        ValidateAmount(amount);
        Balance += amount;
        RecordHistory($"Deposit | {Utils.FormatRupiah(amount)} | {note}");
    }

    public abstract void Withdraw(int amount);
    public abstract int AnnualInterestRate();
    public abstract int AdminFee();

    public virtual void ProcessMonthEnd()
    {
        var oldBalance = Balance;
        var monthlyInterest = AnnualInterestRate() / 12;
        var fee = AdminFee();

        Balance += monthlyInterest;
        RecordHistory($"Monthly Interest | {Utils.FormatRupiah(monthlyInterest)} | Balance : {Utils.FormatRupiah(Balance)}");
        Balance -= fee;
        RecordHistory($"Monthly Fee | {Utils.FormatRupiah(fee)} | Balance : {Utils.FormatRupiah(Balance)}");

        Console.WriteLine("==================================");
        Console.WriteLine($"Update bulanan untuk : {OwnerName} | {AccountNumber}");
        Console.WriteLine($"Saldo awal  : {Utils.FormatRupiah(oldBalance)}\n" +
                          $"Bunga       : {Utils.FormatRupiah(monthlyInterest)}\n" +
                          $"Biaya admin : {Utils.FormatRupiah(fee)}\n" +
                          $"Saldo baru  : {Utils.FormatRupiah(Balance)}");
    }

    public void PrintStatement()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("Belum ada transaksi.");
            return;
        }

        for (var i = 0; i < _history.Count; i++) Console.WriteLine($"{i + 1}. {_history[i]}");
    }

    public virtual void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} | Saldo : {Utils.FormatRupiah(Balance)}");
    }

    protected void RecordHistory(string text)
    {
        _history.Add(text);
    }

    protected static void ValidateAmount(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Jumlah harus lebih dari 0.");
    }
}