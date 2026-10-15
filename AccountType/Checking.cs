using Bank.Abstract;
using Bank.Class;

namespace Bank.AccountType;

public class Checking(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int OverdraftLimit = -1000000;
    
    public override bool Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        } 
        if (Balance - amount < OverdraftLimit)
        {
            Console.WriteLine($"Penarikan melebihi limit overdraft : Rp. {OverdraftLimit}");
            return false;
        }
        Balance -= amount;
        RecordHistory($"Withdraw : {Utils.FormatRupiah(amount)}");
        return true;
    }

    public override int AnnualInterestRate()
    {
        return 0;
    }

    public override int AdminFee()
    {
        return 25000;
    }

    public bool Transfer(Account target, int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        } 
        if (Balance - amount < OverdraftLimit)
        {
            Console.WriteLine($"Penarikan melebihi limit overdraft : Rp. {OverdraftLimit}");
            return false;
        }
        Balance -= amount;
        target.Deposit(amount);
        RecordHistory($"Transfer Out : {Utils.FormatRupiah(amount)}");
        return true;
    }
}