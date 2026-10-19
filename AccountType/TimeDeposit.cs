using PracticeBank.Abstract;
using PracticeBank.Exceptions;

namespace PracticeBank.AccountType;

public class TimeDeposit(string accountNumber, string ownerName, int balance, int monthsToMaturity)
    : Account(accountNumber, ownerName, balance)
{
    public int MonthsToMaturity { get; private set; } = monthsToMaturity;

    public override bool Withdraw(int amount)
    {
        ValidateAmount(amount);

        if (MonthsToMaturity > 0) throw new DepositLockedException(MonthsToMaturity);
        if (amount > Balance) throw new InsufficientBalanceException(Balance, amount);

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

    // Tiap akhir bulan sisa tenor berkurang sehingga deposito akhirnya bisa ditarik
    public override void ProcessMonthEnd()
    {
        base.ProcessMonthEnd();
        if (MonthsToMaturity > 0) MonthsToMaturity--;
    }

    public override void Info()
    {
        Console.WriteLine($"Pemilik: {OwnerName} | {AccountNumber} | Saldo : {Utils.FormatRupiah(Balance)} " +
                          $"| Jatuh tempo dalam {MonthsToMaturity} bulan");
    }
}