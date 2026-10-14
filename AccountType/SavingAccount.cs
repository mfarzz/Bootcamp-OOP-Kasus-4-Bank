using Bank.Abstract;

namespace Bank.AccountType;

public class SavingAccount(string accountNumber, string customerName, double balance)
    : Account(accountNumber, customerName, balance), ITransferable
{
    private const double MinBalance = 50000;

    protected override double YearlyInterest => 0.02;

    protected override double MonthlyFee => 2000;

    protected override bool CanWithdraw(double amount)
    {
        return amount > 0 && base.Balance - amount >= MinBalance;
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