using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Customer
{
    private readonly List<Account> accounts = new();

    public Customer(string name, string customerId)
    {
        CustomerId = customerId;
        Name = name;
    }

    public string CustomerId { get; private set; }
    public string Name { get; set; }
    public IReadOnlyList<Account> Accounts => accounts;
    public int AccountCount => accounts.Count;

    public int TotalAssets
    {
        get
        {
            var total = 0;
            foreach (var account in accounts) total += account.Balance;
            return total;
        }
    }
    
    public double AverageBalance()
    {
        if (accounts.Count == 0)
        {
            return 0;
        }
        return accounts.Average(a => a.Balance);
    }

    public bool AddAccount(Account? account)
    {
        if (account == null) return false;

        foreach (var existing in accounts)
            if (existing.AccountNumber == account.AccountNumber)
                return false;

        accounts.Add(account);
        return true;
    }

    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name} ({CustomerId})");
        Console.WriteLine("Daftar Rekening");
        if (accounts.Count == 0)
        {
            Console.WriteLine("Tidak ada rekening yang terdaftar");
            return;
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("No.  | No. Rekening      | Pemilik   | Saldo");
        Console.WriteLine("-------------------------------------------------------");
        for (var i = 0; i < accounts.Count; i++)
        {
            var account = accounts[i];
            Console.WriteLine(
                $"{i + 1,-4} | {account.AccountNumber,-17} | {account.OwnerName,-9} | {Utils.FormatRupiah(account.Balance)}");
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"Total aset : {Utils.FormatRupiah(TotalAssets)}");
    }
}