using Bank.Abstract;

namespace Bank.AccountType;

public class CheckingAccount(string accountNumber, string customerName, double balance)
    : Account(accountNumber, customerName, balance), ITransferable
{
    private const double OverdraftLimit = -1000000;


    protected override double YearlyInterest => 0;
    protected override double MonthlyFee => 2000;
    

    protected override bool CanWithdraw(double amount)
    {
        return Balance - amount >= OverdraftLimit && amount > 0;
    }

    public bool TransferTo(Account destination, double amount)
    {
        if (!CanWithdraw(amount))
        {
            return false;
        }
        Balance -= amount;
        PushTransaction("Transfer Out", amount, Balance);
        destination.AcceptTransfer(amount);
        return true;
    }
}