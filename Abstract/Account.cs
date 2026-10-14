using Bank.Class;

namespace Bank.Abstract;

public abstract class Account(string accountNumber, Customer customer, double balance)
{
    
    public string AccountNumber { get; } = accountNumber;
    private Customer _customer = customer;
    protected double Balance = balance;
    private readonly Transaction[] _history = new Transaction[10];
    protected abstract double YearlyInterest { get; }
    protected abstract double MonthlyFee { get; }
    public abstract string AccountType { get; }

    public bool Deposit(double amount)
    {
        Balance += amount;
        PushTransaction("Deposit", amount, Balance);
        return true;
    }

    public bool Withdraw(double amount)
    {
        if (!CanWithdraw(amount)) return false;
        Balance -= amount;
        PushTransaction("Withdraw", amount, Balance);
        return true;
    }

    protected void PushTransaction(string type, double amount, double balanceAfter)
    {
        Transaction newTransaction = new Transaction(type, amount, balanceAfter);
        for (var i = 0; i < _history.Length - 1; i++)
        {
            _history[i] = _history[i + 1];
        }

        _history[9] = newTransaction;
    }

    public void PrintHistory()
    {
        foreach (var transaction in _history)
        {
            transaction.PrintStatement();
        }
    }

    public void ApplyEndMonth()
    {
        var oldBalance = Balance;
        double monthlyInterest = Balance *  (YearlyInterest / 12);
        Balance += monthlyInterest;
        PushTransaction("Monthly Interest", monthlyInterest, Balance);
        Balance -= MonthlyFee;
        PushTransaction("Monthly Fee", MonthlyFee, Balance);
        
        Console.WriteLine("==================================");
        Console.WriteLine($"Monthly Update for : {AccountType} | {AccountNumber}");
        Console.WriteLine($"OldBalance: {oldBalance}\nInterest: {monthlyInterest}\nFee: {MonthlyFee}\nBalance: {Balance}");
    }
    protected abstract bool CanWithdraw(double amount);
    
    // testing
    public double GetBalance()
    {
        return Balance;
    }

    public void AcceptTransfer(double amount)
    {
        Balance += amount;
        PushTransaction("Transfer In", amount, Balance);
    }
}