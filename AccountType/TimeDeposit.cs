using Bank.Abstract;
using Bank.Class;
using Bank.Exceptions;

namespace Bank.AccountType;

public class TimeDeposit(string accountNumber, string ownerName, int balance, int monthsToMaturity)
    : Account(accountNumber, ownerName, balance)
{
    public int MonthsToMaturity = monthsToMaturity;
    public override string Type { get; protected set; } = "Time Deposit";
    
    public override bool Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        } 
        if (MonthsToMaturity > 0)
        {
            throw new DepositLockException($"Wait for : {MonthsToMaturity} months to withdraw.");
        }
        Balance -= amount;
        RecordHistory(amount, Balance, "Withdraw");
        return true;
    }

    public override int AnnualInterestRate()
    {
        return (int)(Balance * 0.06);
    }

    public override int AdminFee()
    {
        return 0;
    }

    public override void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} |  Saldo : {Utils.FormatRupiah(Balance)} "
                          + "| Jatuh tempo dalam {MonthsToMaturity} bulan");
    }
}