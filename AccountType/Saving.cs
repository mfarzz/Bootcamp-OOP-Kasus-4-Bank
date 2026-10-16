using PracticeBank.Abstract;

namespace PracticeBank.AccountType;

public class Saving(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int MinimumBalance = 50000;

    public bool Transfer(Account target, int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        }

        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine($"Saldo minimal: Rp. {Utils.FormatRupiah(MinimumBalance)}");
            return false;
        }

        Balance -= amount;
        RecordHistory($"Transfer Out : {Utils.FormatRupiah(amount)}");
        target.Deposit(amount);
        return true;
    }

    public override bool Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        }

        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine($"Saldo minimal: Rp. {Utils.FormatRupiah(MinimumBalance)}");
            return false;
        }

        Balance -= amount;
        RecordHistory($"Withdraw : {Utils.FormatRupiah(amount)}");
        return true;
    }

    public override int AnnualInterestRate()
    {
        return (int)(Balance * 0.02);
    }

    public override int AdminFee()
    {
        return 5000;
    }
}