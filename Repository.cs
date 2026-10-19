using Bank.Abstract;

namespace Bank;

public class Repository<T> where T : Account
{
    private readonly List<T> _items = new();
    private readonly Dictionary<string, T> _accounts = new();
    
    public List<T> GetAll()
    {
        return _items;
    }

    public T? FindByAccountNumber(string accountNumber)
    {
        return _accounts.ContainsKey(accountNumber) ? _accounts[accountNumber] : null;
    }
    
    public void AddAccount(T account)
    {
        if (_accounts.ContainsKey(account.AccountNumber))
        {
            Console.WriteLine("Account number already exists");
            return;
        }
        _items.Add(account);
        _accounts.Add(account.AccountNumber, account);
    }
}