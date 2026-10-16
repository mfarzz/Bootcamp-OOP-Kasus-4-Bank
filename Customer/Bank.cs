using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Bank
{
    public const int LargeTransactionThreshold = 50_000_000;

    private readonly Repository<Account> _accounts = new();
    private readonly List<Customer> _customers = new();
    private readonly HashSet<string> _blockedAccounts = new();
    private readonly List<string> _largeTransactions = new();

    public IReadOnlyList<Customer> Customers => _customers;
    public IReadOnlyList<Account> Accounts => _accounts.All;
    public IReadOnlyList<string> LargeTransactions => _largeTransactions;

    public void AddCustomer(Customer customer)
    {
        _customers.Add(customer);
    }
    
    public bool OpenAccount(Customer customer, Account account)
    {
        if (_accounts.FindByNumber(account.AccountNumber) != null) return false;
        if (!customer.AddAccount(account)) return false;

        _accounts.Add(account);
        return true;
    }

    public Account? FindAccount(string accountNumber)
    {
        return _accounts.FindByNumber(accountNumber);
    }
    
    public bool BlockAccount(string accountNumber)
    {
        return FindAccount(accountNumber) != null && _blockedAccounts.Add(accountNumber);
    }

    public bool UnblockAccount(string accountNumber)
    {
        return _blockedAccounts.Remove(accountNumber);
    }

    public bool IsBlocked(string accountNumber)
    {
        return _blockedAccounts.Contains(accountNumber);
    }
        
    public void Deposit(Account account, int amount)
    {
        EnsureNotBlocked(account);
        account.Deposit(amount);
        DetectLargeTransaction("Deposit", account, amount);
    }

    public void Withdraw(Account account, int amount)
    {
        EnsureNotBlocked(account);
        account.Withdraw(amount);
        DetectLargeTransaction("Withdraw", account, amount);
    }

    public void TransferMoney(Account source, Account destination, int amount)
    {
        if (ReferenceEquals(source, destination))
            throw new ArgumentException("Rekening tujuan tidak boleh sama dengan rekening asal.");

        if (source is not ITransferable transferable)
            throw new InvalidOperationException($"Rekening {source.AccountNumber} tidak mendukung transfer.");

        EnsureNotBlocked(source);
        EnsureNotBlocked(destination);

        transferable.Transfer(destination, amount);
        DetectLargeTransaction("Transfer", source, amount);
    }

    public void ProcessMonthEnd()
    {
        foreach (var account in _accounts.All)
        {
            if (IsBlocked(account.AccountNumber))
            {
                Console.WriteLine($"Rekening {account.AccountNumber} diblokir, proses akhir bulan dilewati.");
                continue;
            }

            account.ProcessMonthEnd();
        }
    }

    private void EnsureNotBlocked(Account account)
    {
        if (IsBlocked(account.AccountNumber))
            throw new InvalidOperationException($"Rekening {account.AccountNumber} sedang diblokir. Transaksi ditolak.");
    }

    private void DetectLargeTransaction(string type, Account account, int amount)
    {
        if (amount < LargeTransactionThreshold) return;

        _largeTransactions.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} | {type} | {account.AccountNumber} | " +
                               $"{account.OwnerName} | {Utils.FormatRupiah(amount)}");
        Console.WriteLine($"[PERINGATAN] Transaksi besar terdeteksi: {type} {Utils.FormatRupiah(amount)} " +
                          $"pada rekening {account.AccountNumber}");
    }
}