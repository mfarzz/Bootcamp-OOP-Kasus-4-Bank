using PracticeBank.Abstract;

namespace PracticeBank.AccountType;

public class TimeDeposit(string accountNumber, string ownerName, int balance, int monthsToMaturity)
    : Account(accountNumber, ownerName, balance)
{
    public int MonthsToMaturity = monthsToMaturity;

    public override bool Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        }

        if (MonthsToMaturity > 0)
        {
            Console.WriteLine($"Jatuh tempo (dalam bulan) : {MonthsToMaturity}");
            return false;
        }

        Balance -= amount;
        RecordHistory($"Withdraw : {Utils.FormatRupiah(amount)}");
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