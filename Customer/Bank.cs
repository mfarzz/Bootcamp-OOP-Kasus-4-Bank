using Bank.Abstract;
using Bank.AccountType;

namespace Bank.Class;

public class Bank
{
    private List<Customer> customers;
    
    private HashSet<Account> blockedAccounts = new();
    
    private readonly Repository<Account> _accountRepository = new();
    // private readonly Repository<Saving> _savingRepository = new();
    // private readonly Repository<Checking> _checkingRepository = new();
    // private readonly Repository<TimeDeposit> _timeDepositRepository = new();

    public Bank()
    {
        Customer c1 = new Customer("Abiem", "CUST01");
        Customer c2 = new Customer("Fariz", "CUST02");
        
        Saving sa1 = new Saving("sa-01", "Abiem", 200000);
        Checking ca1 = new Checking("ca-01", "Abiem", 1500000);
        TimeDeposit tda1 = new TimeDeposit("tda-01", "Abiem", 10000000, 6);

        Saving sa2 = new Saving("sa-02", "Fariz", 150000);
        Checking ca2 = new Checking("ca-02", "Fariz", 800000);
        TimeDeposit tda2 = new TimeDeposit("tda-02", "Fariz", 2000000, 6);
        
        c1.AddAccount(sa1);
        c1.AddAccount(ca1);
        c1.AddAccount(tda1);

        c2.AddAccount(sa2);
        c2.AddAccount(ca2);
        c2.AddAccount(tda2);

        c1.GetAccounts()[0].Deposit(500000);
        c1.GetAccounts()[0].Deposit(80000);
        c1.GetAccounts()[0].Deposit(1000000);
        c1.GetAccounts()[0].Withdraw(250000);
        c1.GetAccounts()[0].Deposit(600000);
        c1.GetAccounts()[0].Deposit(200000);
        c1.GetAccounts()[0].Deposit(400000);
        c1.GetAccounts()[0].Deposit(50000);
        c1.GetAccounts()[0].Withdraw(230000);
        
        customers = new List<Customer> { c1, c2 };
        
        _accountRepository.AddAccount(sa1);
        _accountRepository.AddAccount(sa2);
        _accountRepository.AddAccount(ca1);
        _accountRepository.AddAccount(ca2);
        _accountRepository.AddAccount(tda1);
        _accountRepository.AddAccount(tda2);
        
        // _checkingRepository.AddAccount(ca1);
        // _checkingRepository.AddAccount(ca2);
        //
        // _savingRepository.AddAccount(sa1);
        // _savingRepository.AddAccount(sa2);
        //
        // _timeDepositRepository.AddAccount(tda1);
        // _timeDepositRepository.AddAccount(tda2);
    }

    public Account? FindAccount(string accountNumber)
    {
        return _accountRepository.FindByAccountNumber(accountNumber);
    }

    public void ProcessMonthEnd()
    {
        foreach (var acc in _accountRepository.GetAll())
        {
            acc.ProcessMonthEnd();
        }
    }

    public List<Customer> GetAllCustomers()
    {
        return customers;
    }

    public List<Account> GetAllAccounts()
    {
        return _accountRepository.GetAll();
    }

    public void BalancePerAccountType()
    {
        var accounts = _accountRepository.GetAll();
        var res = accounts.GroupBy(a => a.Type)
            .Select(g => new { Type = g.Key, TotalBalance = g.Sum(a => a.Balance) });
        foreach (var item in res)
        {
            Console.WriteLine($"{item.Type}: {item.TotalBalance}");
        }
    }

    public Customer? CustomerWithHighestAssets()
    {
        var highestAssets = customers.Max(c => c.TotalAssets);
        var res = customers.FirstOrDefault(c =>c.TotalAssets == highestAssets);
        return res;
    }
}