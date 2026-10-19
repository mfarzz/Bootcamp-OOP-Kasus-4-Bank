using Bank.Abstract;
using Bank.Class;
using Bank.Exceptions;

namespace Bank.AccountType;

public class Saving(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int MinimumBalance = 50000;
    public override string Type { get; protected set; } = "Saving";

    public async Task<bool> Transfer(Account target, int amount, CancellationToken ct)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Nominal harus lebih besar dari 0");
            return false;
        } 
        if (Balance - amount < MinimumBalance)
        {
            throw new InsufficientBalanceException("InsufficientBalance", Balance, amount);
        }
        
        ct.ThrowIfCancellationRequested();
        await Task.Delay(500, ct);
        
        Balance -= amount;
        RecordHistory(amount, Balance, "Transfer out");
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
            throw new InsufficientBalanceException("InsufficientBalance", Balance, amount);
        }
        Balance -= amount;
        RecordHistory(amount, Balance, "Withdraw");
        return true;
    }

    public override int AnnualInterestRate()
    {
        return (int) (Balance * 0.02);
    }

    public override int AdminFee()
    {
        return 5000;
    }
}