using PracticeBank.Abstract;

namespace PracticeBank.Class;

public class Bank
{
    private Account[] accounts;
    private Customer[] customers;

    public Bank(Customer[] customers, params Account[][] accountsPerCustomer)
    {
        if (customers.Length != accountsPerCustomer.Length)
        {
            Console.WriteLine("Invalid amount of customers");
            return;
        }
 
        this.customers = customers;
 
        var total = 0;
        foreach (var group in accountsPerCustomer) total += group.Length;
 
        accounts = new Account[total];
        var index = 0;

        for (var i = 0; i < accountsPerCustomer.Length; i++)
        {
            foreach (var acc in accountsPerCustomer[i])
            {
                this.customers[i].AddAccount(acc);
                accounts[index++] = acc;
            }
        }
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