using PracticeBank.Abstract;
using PracticeBank.Exceptions;

namespace PracticeBank.AccountType;

public class Saving(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int MinimumBalance { get; } = 50000;

    public override void Withdraw(int amount)
    {
        Debit(amount, "Withdraw");
    }

    public bool Transfer(Account target, int amount)
    {
        Debit(amount, $"Transfer Out ke {target.AccountNumber}");
        target.Deposit(amount, $"Transfer In dari {AccountNumber}");
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

    private void Debit(int amount, string label)
    {
        ValidateAmount(amount);

        if (Balance - amount < MinimumBalance)
            throw new InsufficientBalanceException(Balance, amount,
                $"Saldo minimal {Utils.FormatRupiah(MinimumBalance)} harus tersisa.");

        Balance -= amount;
        RecordHistory($"{label} : {Utils.FormatRupiah(amount)}");
    }
}