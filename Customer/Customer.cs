using Bank.Abstract;

namespace Bank.Class;

public class Customer
{
    public string CustomerId { get; private set; }
    public string Name { get; set; }
    private Account[] accounts = new Account[3];
    private int accountCount;

    public int TotalAssets
    {
        get
        {
            int total = 0;
            for (int i = 0; i < accountCount; i++)
            {
                total += accounts[i].Balance;
            }
            return total;
        }
    }

    public Customer(string name, string customerId)
    {
        CustomerId = customerId;
        Name = name;
        accountCount = 0;
    }

    public bool AddAccount(Account? account)
    {
        if (account == null) return false;
        
        if (accountCount >= accounts.Length) return false;
    
        for (int i = 0; i < accountCount; i++)
        {
            if (accounts[i].AccountNumber == account.AccountNumber)
            {
                return false;
            }
        }
        accounts[accountCount] = account;
        accountCount++;
        
        return true;
    }
    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name}");
        Console.WriteLine("Daftar Rekening");
        if (accountCount == 0)
        {
            Console.WriteLine("Tidak ada rekening yang terdaftar");
            return;
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("No.  | No. Rekening      | Pemilik   | Saldo");
        Console.WriteLine("-------------------------------------------------------");
        for (int i = 0; i < accountCount; i++)
        {
            Account account = accounts[i];
            Console.WriteLine($"{i + 1, -4}. {account.AccountNumber, -17} | {Name, -9} | {Utils.FormatRupiah(account.Balance)}");
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"Total aset : {Utils.FormatRupiah(TotalAssets)}");
    }
    
}