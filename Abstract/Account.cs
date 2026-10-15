using Bank.Class;

namespace Bank.Abstract;

public abstract class Account(string accountNumber, string customerName, int balance)
{
    public static int TotalAccount = 0;
    public string AccountNumber { get; private set; } = accountNumber;
    public string OwnerName = customerName;
    public int Balance { get; set; } = balance;
    private string[] _history = new string[10];
    private int historyCount;
    // protected abstract double YearlyInterest { get; }
    // protected abstract double MonthlyFee { get; }
    // public abstract string AccountType { get; }
    // private static readonly int MaxHistory = 10;

    public bool Deposit(int amount)
    {
        if (amount <= 0) return false;
        Balance += amount;
        RecordHistory($"Deposit : {Utils.FormatRupiah(amount)}");
        return true;
    }

    public bool Deposit(int amount, string note)
    {
        if (amount <= 0) return false;
        Balance += amount;
        RecordHistory($"Deposit | {Utils.FormatRupiah(amount)} | {note}");
        return true;
    }

    public abstract bool Withdraw(int amount);
    public abstract int AnnualInterestRate();
    public abstract int AdminFee();

    public void ProcessMonthEnd()
    {
        var oldBalance = Balance;
        // ReSharper disable once PossibleLossOfFraction
        int monthlyInterest = AnnualInterestRate()/12;
        Balance += monthlyInterest;
        RecordHistory($"Monthly Interest | {Utils.FormatRupiah(monthlyInterest)} | Balance : {Utils.FormatRupiah(Balance)}");
        Balance -= AdminFee();
        RecordHistory($"Monthly Fee | {Utils.FormatRupiah(AdminFee())} | Balance : {Balance}");
        
        Console.WriteLine("==================================");
        Console.WriteLine($"Update bulanan untuk : {OwnerName} | {AccountNumber}");
        Console.WriteLine($"Saldo awal: Rp. {Utils.FormatRupiah(oldBalance)}\nBunga: Rp. {Utils.FormatRupiah(monthlyInterest)}\n"+
                          $"Biaya admin: Rp. {Utils.FormatRupiah(AdminFee())}\nSaldo Baru: Rp. {Utils.FormatRupiah(Balance)}");
        
    }
    
    protected void RecordHistory(string text)
    {
        for (var i = 0; i < _history.Length - 1; i++)
        {
            _history[i] = _history[i + 1];
        }

        _history[9] = text;
    }

    public void PrintStatement()
    {
        foreach (var transaction in _history)
        {
            Console.WriteLine(transaction);
        }
    }

    public virtual void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} |  Saldo : {Utils.FormatRupiah(Balance)}");
    }
}