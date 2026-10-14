using Bank.Abstract;
using Bank.AccountType;

namespace Bank;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Account save = new SavingAccount("123", "Adi", 1000000);
        save.ApplyEndMonth();
        Console.WriteLine(save.GetBalance());
    }
}