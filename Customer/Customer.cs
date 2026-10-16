using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Customer(string name, string customerId)
{
    private readonly List<Account> _accounts = new();
    public string CustomerId { get; } = customerId;
    public string Name { get; set; } = name;
    public IReadOnlyList<Account> Accounts => _accounts;

    public int TotalAssets
    {
        get
        {
            var total = 0;
            foreach (var account in _accounts) total += account.Balance;
            return total;
        }
    }

    public bool AddAccount(Account? account)
    {
        if (account == null) return false;

        foreach (var existing in _accounts)
            if (existing.AccountNumber == account.AccountNumber)
                return false;

        _accounts.Add(account);
        return true;
    }

    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name} ({CustomerId})");
        Console.WriteLine("Daftar Rekening");
        if (_accounts.Count == 0)
        {
            Console.WriteLine("Tidak ada rekening yang terdaftar");
            return;
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("No.  | No. Rekening      | Pemilik   | Saldo");
        Console.WriteLine("-------------------------------------------------------");
        for (var i = 0; i < _accounts.Count; i++)
        {
            var account = _accounts[i];
            Console.WriteLine(
                $"{i + 1,-4} | {account.AccountNumber,-17} | {account.OwnerName,-9} | {Utils.FormatRupiah(account.Balance)}");
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"Total aset : {Utils.FormatRupiah(TotalAssets)}");
    }
}