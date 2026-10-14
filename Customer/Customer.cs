using Bank.Abstract;

namespace Bank.Class;

public class Customer
{
    private const int MaxAccounts = 5;
    public string Name { get; set; }
    private Account[] accounts;
    public int AccountCount { get; private set; }
    
    public Customer(string name)
    {
        Name = name;
        this.accounts = new Account[MaxAccounts];
        AccountCount = 0;
    }

    public bool AddAccount(Account account)
    {
        if (account is null) return false;
        if (AccountCount >= MaxAccounts) return false;
    
        for (int i = 0; i < AccountCount; i++)
        {
            if (accounts[i].AccountNumber == account.AccountNumber)
            {
                return false;
            }
        }
        accounts[AccountCount] = account;
        AccountCount++;
        return true;
    }

    public double GetTotalAssets()
    {
        double total = 0;
        for (int i = 0; i < AccountCount; i++)
        {
            total += accounts[i].GetBalance();
        }
        return total;
    }

    public void PrintProfile()
    {
        Console.WriteLine($"Nasabah : {Name}");
        Console.WriteLine("Daftar Rekening");
        for (int i = 0; i < AccountCount; i++)
        {
            Account account = accounts[i];
            Console.WriteLine($"    {i + 1}. {account.AccountNumber}    |   {Name}    |   Rp{account.GetBalance:NO}");
        }
    
        Console.WriteLine($"Total aset : Rp{GetTotalAssets():NO}");
    }

    public void ApplyEndMonthAll()
    {
        for (int i = 0; i < AccountCount; i++)
        {
            accounts[i].ApplyEndMonth();
        }
    }
}