using PracticeBank.Abstract;
using PracticeBank.Exceptions;

namespace PracticeBank.AccountType;

public class Checking(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public int OverdraftLimit { get; } = -1000000;

    public override bool Withdraw(int amount)
    {
        Debit(amount, "Withdraw");
        return true;
    }

    public bool Transfer(Account target, int amount)
    {
        Debit(amount, $"Transfer Out ke {target.AccountNumber}");
        target.Deposit(amount, $"Transfer In dari {AccountNumber}");
        return true;
    }

    public async Task<bool> TransferInterbankAsync(string bankName, string destinationAccountNumber, int ammount,
        CancellationToken cancellationToken)
    {
        CanDebit(ammount);
        await SimulateInterbankLatencyAsync(cancellationToken);
        Debit(ammount, $"Transfer antar bank ke {bankName} ({destinationAccountNumber})");
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

    private void Debit(int amount, string label)
    {
        CanDebit(amount);
        Balance -= amount;
        RecordHistory($"{label} : {Utils.FormatRupiah(amount)}");
    }

    private void CanDebit(int amount)
    {
        ValidateAmount(amount);
        if (Balance - amount < OverdraftLimit)
            throw new InsufficientBalanceException(Balance, amount,
                $"Limit overdraft {Utils.FormatRupiah(OverdraftLimit)}.");
    }
}