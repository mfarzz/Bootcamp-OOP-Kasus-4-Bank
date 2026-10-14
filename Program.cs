using Bank.Abstract;
using Bank.AccountType;
using Bank.Class;

namespace Bank;

class Program
{
    static void Main(string[] args)
    {
        Customer c1 = new Customer("John");
        Account a1 = new SavingAccount("John", "John", 1000);
        Console.WriteLine("Hello, World!");
        Account save = new SavingAccount("123", "Adi", 1000000);
        save.ApplyEndMonth();
        Console.WriteLine(save.GetBalance());
    }
}