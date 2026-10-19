namespace PracticeBank.Abstract;

public abstract class Account
{
    public static int TotalAccounts = 0;

    private readonly List<string> history = new();

    protected Account(string accountNumber, string ownerName, int balance)
    {
        AccountNumber = accountNumber;
        OwnerName = ownerName;
        Balance = balance;
        TotalAccounts++;
    }

    public string AccountNumber { get; private set; }
    public string OwnerName { get; }
    public int Balance { get; protected set; }

    // Setor 0 atau negatif melempar ArgumentException
    public bool Deposit(int amount)
    {
        ValidateAmount(amount);
        Balance += amount;
        RecordHistory($"Deposit : {Utils.FormatRupiah(amount)}");
        return true;
    }

    public bool Deposit(int amount, string note)
    {
        ValidateAmount(amount);
        Balance += amount;
        RecordHistory($"Deposit | {Utils.FormatRupiah(amount)} | {note}");
        return true;
    }

    // true jika berhasil; melempar InsufficientBalanceException / DepositLockedException / ArgumentException
    public abstract bool Withdraw(int amount);
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

    protected void RecordHistory(string text)
    {
        history.Add(text);
    }

    public void PrintStatement()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("Belum ada transaksi.");
            return;
        }

        for (var i = 0; i < history.Count; i++) Console.WriteLine($"{i + 1}. {history[i]}");
    }

    public virtual void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} | Saldo : {Utils.FormatRupiah(Balance)}");
    }

    protected static void ValidateAmount(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Jumlah harus lebih dari 0.");
    }
}