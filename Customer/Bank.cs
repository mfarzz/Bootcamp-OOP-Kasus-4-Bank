using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Bank
{
    // Transaksi dengan nominal >= batas ini dicatat sebagai transaksi besar
    public const int LargeTransactionThreshold = 5_000_000;

    private readonly List<Customer> customers = new();
    private readonly Repository<Account> accounts = new();           // List + Dictionary
    private readonly HashSet<string> blockedAccounts = new();        // nomor rekening yang diblokir
    private readonly List<TransactionRecord> transactions = new();

    public Bank(IEnumerable<Customer> customers, IEnumerable<Account> accounts)
    {
        this.customers.AddRange(customers);
        foreach (var account in accounts) this.accounts.Add(account);
    }

    public IReadOnlyList<Customer> Customers => customers;
    public IReadOnlyList<Account> Accounts => accounts.All;

    public Account? FindAccount(string accountNumber)
    {
        return accounts.FindByNumber(accountNumber);
    }
    

    // Proses akhir bulan hanya untuk rekening milik satu nasabah
    public void ProcessMonthEnd(Customer customer)
    {
        foreach (var account in customer.Accounts) ProcessOne(account);
    }

    // Untuk seluruh nasabah
    public Task ProcessMonthEndAsync()
    {
        return ProcessManyAsync(accounts.All);
    }

    private Task ProcessManyAsync(IEnumerable<Account> target)
    {
        var tasks = target.Select(accounts => Task.Run(() => ProcessOne(accounts))).ToList();
        return Task.WhenAll(tasks);
    }

    // Daftarkan rekening baru ke nasabah sekaligus ke daftar rekening bank
    public bool OpenAccount(Customer customer, Account account)
    {
        if (accounts.FindByNumber(account.AccountNumber) != null) return false;
        if (!customer.AddAccount(account)) return false;

        accounts.Add(account);
        return true;
    }

    public bool BlockAccount(string accountNumber)
    {
        return FindAccount(accountNumber) != null && blockedAccounts.Add(accountNumber);
    }

    public bool UnblockAccount(string accountNumber)
    {
        return blockedAccounts.Remove(accountNumber);
    }

    public bool IsBlocked(string accountNumber)
    {
        return blockedAccounts.Contains(accountNumber);
    }

    // Dipanggil sebelum setiap transaksi; rekening terblokir menolak semua transaksi
    public void EnsureNotBlocked(Account account)
    {
        if (IsBlocked(account.AccountNumber))
            throw new InvalidOperationException($"Rekening {account.AccountNumber} sedang diblokir. Transaksi ditolak.");
    }

    public void RecordTransaction(string type, Account account, int amount)
    {
        transactions.Add(new TransactionRecord(DateTime.Now, type, account.AccountNumber, account.OwnerName, amount));
 
        if (amount > LargeTransactionThreshold)
            Console.WriteLine($"[PERINGATAN] Transaksi besar terdeteksi: {type} {Utils.FormatRupiah(amount)} " +
                              $"pada rekening {account.AccountNumber}");
    }
    
    // Transaksi besar pada rekening milik satu nasabah
    public List<TransactionRecord> GetLargeTransactions(Customer customer)
    {
        return transactions
            .Where(t => t.Amount > LargeTransactionThreshold)
            .Where(t=> customer.Accounts.Any(a => a.AccountNumber == t.AccountNumber))
            .OrderByDescending(t => t.Time)
            .ToList();
    }

    private void ProcessOne(Account account)
    {
        if (IsBlocked(account.AccountNumber))
        {
            Console.WriteLine($"Rekening {account.AccountNumber} diblokir, proses akhir bulan dilewati.");
            return;
        }

        account.ProcessMonthEnd();
    }
}