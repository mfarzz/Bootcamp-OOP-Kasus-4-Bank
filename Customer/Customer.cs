using Bank.Abstract;

namespace Bank.Class;

public class Customer : Account
{
    public string Name { get; set; }

    private Account[] accounts = new Account[5];
    
    public Customer (string accountNumber, string customerName, double balance) 
        : base(accountNumber, customerName, balance)
    {
        Name = customerName;
        
        
    }

    public void AddAccount()
    {
        // for (int i; i <)
    }

    public override void ApplyEndMonth()
    {
        throw new NotImplementedException();
    }

    protected override bool CanWithdraw(double amount)
    {
        throw new NotImplementedException();
    }
}