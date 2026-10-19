using System.Text;
using Bank.Class;

namespace Bank.Abstract;

public abstract class Account(string accountNumber, string customerName, int balance)
{
    public static int TotalAccount = 0;
    public string AccountNumber { get; private set; } = accountNumber;
    public string OwnerName = customerName;
    public int Balance { get; protected set; } = balance;
    private List<Transaction> _history = new();
    public abstract string Type {get; protected set;}

    public bool Deposit(int amount)
    {
        if (amount <= 0) return false;
        Balance += amount;
        RecordHistory(amount, Balance, "Deposit");
        return true;
    }

    public bool Deposit(int amount, string note)
    {
        if (amount <= 0) return false;
        Balance += amount;
        RecordHistory(amount, Balance, "Deposit");
        return true;
    }

    public abstract bool Withdraw(int amount);
    public abstract int AnnualInterestRate();
    public abstract int AdminFee();

    public async Task<string> ProcessMonthEnd()
    {
        var oldBalance = Balance;
        // ReSharper disable once PossibleLossOfFraction
        var monthlyInterestTask = ProcessMonthlyInterest();
        var adminFeeTask = ProcessAdminFee();
        
        await Task.WhenAll(monthlyInterestTask, adminFeeTask);

        var monthlyInterest = await monthlyInterestTask;
        var sb = new StringBuilder();
        
        sb.AppendLine("==================================");
        sb.AppendLine($"Update bulanan untuk : {OwnerName} | {AccountNumber}");
        sb.AppendLine($"Saldo awal: Rp. {Utils.FormatRupiah(oldBalance)}\nBunga: Rp. {Utils.FormatRupiah(monthlyInterest)}\n"+
                          $"Biaya admin: Rp. {Utils.FormatRupiah(AdminFee())}\nSaldo Baru: Rp. {Utils.FormatRupiah(Balance)}");

        return sb.ToString();
    }

    private async Task<int> ProcessMonthlyInterest()
    {
        await Task.Delay(500); // simulasi delay
        int monthlyInterest = AnnualInterestRate()/12;
        Balance += monthlyInterest;
        RecordHistory(monthlyInterest, Balance, "Monthly Interest");
        return monthlyInterest;
    }

    private async Task ProcessAdminFee()
    {
        await Task.Delay(500); // simulasi delay
        
        Balance -= AdminFee();
        RecordHistory(AdminFee(), Balance, "Admin Fee");
    }
    
    protected void RecordHistory(int amount, int balanceAfter, string type)
    {
        var tr = new Transaction(amount, balanceAfter, DateTime.Now ,type);
        _history.Add(tr);
    }

    public void PrintStatement()
    {
        var tr = _history.OrderByDescending(t => t.Date)
            .Take(10);
        foreach (var transaction in tr)
        {
            transaction.PrintTransaction();
        }
    }

    public virtual void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} |  Saldo : {Utils.FormatRupiah(Balance)}");
    }
}