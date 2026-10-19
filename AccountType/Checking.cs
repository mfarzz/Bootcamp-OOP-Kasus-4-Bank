using Bank.Abstract;
using Bank.Class;
using Bank.Exceptions;

namespace Bank.AccountType;

public class Checking(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int OverdraftLimit = -1000000;

    public override string Type { get; protected set; } = "Checking";

    public override bool Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        } 
        if (Balance - amount < OverdraftLimit)
        {
            throw new InsufficientBalanceException($"Penarikan melebihi limit overdraft : Rp. {OverdraftLimit}", Balance, amount);
        }
        Balance -= amount;
        RecordHistory(amount, Balance, "Withdraw");
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
        RecordHistory(amount, Balance, "Transfer out");
        return true;
    }
}