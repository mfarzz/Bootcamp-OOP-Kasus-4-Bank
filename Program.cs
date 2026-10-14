using Bank.AccountType;
using Bank.Class;
using Bank.Feature;

namespace Bank;

class Program
{
    static Features features = new Features();
    
    static DateTime maturityDate1 = new DateTime(2026, 10, 15);
    static DateTime maturityDate2 = new DateTime(2026, 10, 13);
    
    static Customer c1 = new Customer("Abiem");
    static Customer c2 = new Customer("Fariz");

    private SavingAccount sa1 = new SavingAccount("sa-01", c1, 200000);
    private CheckingAccount ca1 = new CheckingAccount("ca-01", c1, 1500000);
    private TimeDepositAccount tda1 = new TimeDepositAccount("tda-01", c2, 10000000, maturityDate1);
    
    private SavingAccount sa2 = new SavingAccount("sa-02", c2, 150000);
    private CheckingAccount ca2 = new CheckingAccount("ca-02", c2, 800000);
    private TimeDepositAccount tda2 = new TimeDepositAccount("tda-02", c2, 2000000, maturityDate1);
    
    static void Main(string[] args)
    {
        Customer currentUser = PickUser();
        while (true)
        {
            ShowMenu();
            var option = GetOption();
            switch (option)
            {
                case "1":
                    features.Profil(currentUser);
                    break;
                case "2":
                    features.Deposit(currentUser);
                    break;
                case "3":
                    features.Withdraw(currentUser);
                    break;
            }
        }
    }

    private static Customer PickUser()
    {
        while (true)
        {
            Console.WriteLine($"Registered Customer:\n1. {c1.Name}\n2. {c2.Name}");
            Console.Write("Pick user: ");
            var  input = Console.ReadLine();
            if (input == "1")
            {
                return c1;
            } 
            if (input == "2")
            {
                return c2;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Menu:\n1. Profile\n2. Deposit\n3. Withdraw");
        Console.WriteLine("4. Print Mutation\n5. End Month Process\n0. Switch User");
        Console.Write("Select an option: ");
    }

    private static string? GetOption()
    {
        return Console.ReadLine();
    }
}