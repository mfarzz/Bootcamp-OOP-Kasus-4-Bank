using Bank.Abstract;
using Bank.Class;

namespace Bank.AccountType;

public class SavingAccount(string accountNumber, Customer customer, double balance)
    : Account(accountNumber, customer, balance), ITransferable
{
    private const double MinBalance = 50000;
    public override string AccountType => "Saving";
    protected override double YearlyInterest => 0.02;

    protected override double MonthlyFee => 5000;

    protected override bool CanWithdraw(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
        } else if (Balance - amount < MinBalance)
        {
            Console.WriteLine($"Saldo minimal: Rp. {MinBalance:N}");
        }
        return amount > 0 && Balance - amount >= MinBalance;
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