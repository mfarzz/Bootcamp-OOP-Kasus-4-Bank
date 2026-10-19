using PracticeBank.Abstract;
using PracticeBank.Exceptions;

namespace PracticeBank.AccountType;

public class Savings(string accountNumber, string ownerName, int balance)
    : Account(accountNumber, ownerName, balance), ITransferable
{
    public const int DefaultMinimumBalance = 50000;

    public int MinimumBalance { get; } = DefaultMinimumBalance;

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
        return (int)(Balance * 0.02);
    }

    public override int AdminFee()
    {
        return 5000;
    }
    
    private void CanDebit(int amount)
    {
        ValidateAmount(amount);
        if (Balance - amount < MinimumBalance) throw new InsufficientBalanceException(Balance, amount, $"Saldo minimal {Utils.FormatRupiah(MinimumBalance)} harus tersisa.");
    }
    
    private void Debit(int amount, string label)
    {
        CanDebit(amount);
        Balance -= amount;
        RecordHistory($"{label} : {Utils.FormatRupiah(amount)}");
    }

}