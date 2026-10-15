using Bank.Abstract;
using Bank.Class;

namespace Bank.AccountType;

public class CheckingAccount(string accountNumber, Customer customer, double balance)
    : Account(accountNumber, customer, balance), ITransferable
{
    private const double OverdraftLimit = -1000000;
    public override string AccountType => "Checking";

    protected override double YearlyInterest => 0;
    protected override double MonthlyFee => 25000;
    

    protected override bool CanWithdraw(double amount)
    {
        if (Balance - amount < OverdraftLimit)
        {
            Console.WriteLine($"Penarikan melebihi limit overdraft : Rp. {OverdraftLimit}");
        } else if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0.");
        }
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