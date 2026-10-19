using Bank.Abstract;

namespace Bank.Class;

public class Customer
{
    public string CustomerId { get; private set; }
    public string Name { get; set; }
    private List<Account> accounts;

    public int TotalAssets
    {
        get { return accounts.Sum(a => a.Balance); }
    }

    public Customer(string name, string customerId)
    {
        accounts = new List<Account>();
        CustomerId = customerId;
        Name = name;
    }

    public bool AddAccount(Account? account)
    {
        if (account == null) return false;

        accounts.Add(account);
        return true;
    }

    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name}");
        Console.WriteLine("Daftar Rekening");
        if (accounts.Count < 0)
        {
            Console.WriteLine("Tidak ada rekening yang terdaftar");
            return;
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("No.  | No. Rekening      | Pemilik   | Saldo");
        Console.WriteLine("-------------------------------------------------------");
        var i = 1;
        foreach (var acc in accounts)
        {
            Console.WriteLine($"{i}. {acc.AccountNumber,-17} | {Name,-9} | {Utils.FormatRupiah(acc.Balance)}");
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"Total aset : {Utils.FormatRupiah(TotalAssets)}");
    }

    public List<Account> GetAccounts()
    {
        return accounts;
    }
}