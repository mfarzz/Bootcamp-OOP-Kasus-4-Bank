using Bank.Abstract;
using Bank.Class;

namespace Bank.AccountType;

public class TimeDepositAccount(string accountNumber, Customer customer, double balance)
    : Account(accountNumber, customer, balance)
{
    protected override double YearlyInterest => 0.06;
    protected override double MonthlyFee => 2000;
    public override string AccountType => "Deposit";
    private readonly DateTime _maturityDate = DateTime.Now.AddMonths(6);
    
    protected override bool CanWithdraw(double amount)
    {
        return IsMature() && amount > 0;
    }

    private bool IsMature()
    {
        return DateTime.Now >= _maturityDate;
    }
}