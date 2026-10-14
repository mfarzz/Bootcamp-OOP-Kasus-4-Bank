namespace Bank.Abstract;

public abstract class Account
{
    private string accountNumber;
    private string customerName;
    private double balance;
    private string[] history;
    protected float MonthyInterest { get; }
    protected double MonthlyFee { get; }

    public Account(string accountNumber, string customerName, double balance)
    {
        this.accountNumber = accountNumber;
        this.customerName = customerName;
        this.balance = balance;
        this.history = new string[10];
    }

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
    
    public abstract void ApplyEndMonth();
    protected abstract bool CanWithdraw(double amount);
}