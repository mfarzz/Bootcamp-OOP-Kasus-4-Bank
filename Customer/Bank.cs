using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Bank
{
    private readonly Account[] accounts;
    private Customer[] customers;

    public Bank(Customer[] customers, params Account[][] accountsPerCustomer)
    {
        this.customers = customers;

        var total = 0;
        foreach (var group in accountsPerCustomer) total += group.Length;
        
        accounts = new Account[total];
        var index = 0;
        foreach (var group in accountsPerCustomer)
            foreach (var account in group)
                accounts[index++] = account;
    }

    public Account? FindAccount(string accountNumber)
    {
        foreach (var account in accounts)
            if (account != null && account.AccountNumber == accountNumber)
                return account;

        return null;
    }

    public void ProcessMonthEnd()
    {
        foreach (var account in accounts)
            if (account != null)
                account.ProcessMonthEnd();
    }
}