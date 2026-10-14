using Bank.Abstract;

namespace Bank.AccountType;

public class TimeDepositAccount(string accountNumber, string customerName, double balance, DateTime maturityDate)
    : Account(accountNumber, customerName, balance)
{
    protected override double YearlyInterest => 0.06;
    protected override double MonthlyFee => 2000;
    private readonly DateTime _maturityDate = maturityDate;
    
    protected override bool CanWithdraw(double amount)
    {
        return IsMature() && amount > 0;
    }

    private bool IsMature()
    {
        return DateTime.Now >= _maturityDate;
    }
}