namespace Bank.Abstract;

public abstract class Account(string accountNumber, string customerName, double balance)
{
    private string _accountNumber = accountNumber;
    private string _customerName = customerName;
    protected double Balance = balance;
    private string[] _history = new string[10];
    protected abstract double YearlyInterest { get; }
    protected abstract double MonthlyFee { get; }

    public bool Deposit(double amount)
    {
        // TODO
        return true;
    }

    public bool Withdraw(double amount)
    {
        // TODO
        return true;
    }

    public void PushTransaction(string type, double amount, double balanceAfter)
    {
        // TODO
    }

    public void PrintHistory()
    {
        // TODO
    }

    public void ApplyEndMonth()
    {
        double monthlyInterest = Balance *  (YearlyInterest / 12);
        // Console.WriteLine($"interest: {monthlyInterest}\nFee: {MonthlyFee}");
        Balance = Balance + monthlyInterest - MonthlyFee;
    }
    protected abstract bool CanWithdraw(double amount);
    
    // testing
    public double GetBalance()
    {
        return Balance;
    }
}