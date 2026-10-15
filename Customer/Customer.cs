using Bank.Abstract;

namespace Bank.Class;

public class Customer
{
    private const int MaxAccounts = 5;
    public string Name { get; set; }
    public Account[] Accounts {get; private set;}
    public int AccountCount { get; private set; }
    
    public Customer(string name)
    {
        Name = name;
        Accounts = new Account[MaxAccounts];
        AccountCount = 0;
    }

    public bool AddAccount(Account? account)
    {
        if (account == null) return false;
        if (AccountCount >= MaxAccounts) return false;
    
        for (int i = 0; i < AccountCount; i++)
        {
            if (Accounts[i].AccountNumber == account.AccountNumber)
            {
                return false;
            }
        }
        Accounts[AccountCount] = account;
        AccountCount++;
        return true;
    }

    private double GetTotalAssets()
    {
        double total = 0;
        for (int i = 0; i < AccountCount; i++)
        {
            total += Accounts[i].GetBalance();
        }
        return total;
    }

    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name}");
        Console.WriteLine("Daftar Rekening");
        if (AccountCount == 0)
        {
            Console.WriteLine("Tidak ada rekening yang terdaftar");
            return;
        }
        for (int i = 0; i < AccountCount; i++)
        {
            Account account = Accounts[i];
            Console.WriteLine($"    {i + 1}. {account.AccountNumber}    |   {Name}    |   Rp{account.GetBalance():N}");
        }
    
        Console.WriteLine($"Total aset : Rp{GetTotalAssets():N}");
    }

    public void ApplyEndMonthAll()
    {
        for (int i = 0; i < AccountCount; i++)
        {
            Accounts[i].ApplyEndMonth();
        }
    }
}