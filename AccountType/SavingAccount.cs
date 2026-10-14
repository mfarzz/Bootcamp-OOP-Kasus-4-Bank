using Bank.Abstract;

namespace Bank.AccountType;

public class SavingAccount(string accountNumber, string customerName, double balance)
    : Account(accountNumber, customerName, balance)
{
    private const double MinBalance = 50000;

    protected override double YearlyInterest => 0.02;

    protected override double MonthlyFee => 2000;

    protected override bool CanWithdraw(double amount)
    {
        return amount > 0 && Balance - amount >= MinBalance;
    }
}