using Bank.Abstract;

namespace Bank.Class;

public class Bank
{
    private Customer[] customers;
    private Account[] accounts;

    public Account? FindAccount(string accountNumber)
    {
        foreach (Account? account in accounts)
        {
            if (account != null && account.AccountNumber == accountNumber)
            {
                return account;
            }  
        }
        return null;
    }

    public void ProcessMonthEnd()
    {
        foreach (Account? account in accounts)
        {
            if (account != null)
            {
                // account.ProcessMonthEnd(); //tunggu implementasi di account
            }
        }
    }
}