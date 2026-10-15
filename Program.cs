using Bank.AccountType;
using Bank.Class;
using Bank.Feature;

namespace Bank;

class Program
{
    static Customer c1 = new Customer("Abiem");
    static Customer c2 = new Customer("Fariz");
    static Customer[] allCustomers = { c1, c2 };

    private SavingAccount sa1 = new SavingAccount("sa-01", c1, 200000);
    private CheckingAccount ca1 = new CheckingAccount("ca-01", c1, 1500000);
    private TimeDepositAccount tda1 = new TimeDepositAccount("tda-01", c2, 10000000);
    
    private SavingAccount sa2 = new SavingAccount("sa-02", c2, 150000);
    private CheckingAccount ca2 = new CheckingAccount("ca-02", c2, 800000);
    private TimeDepositAccount tda2 = new TimeDepositAccount("tda-02", c2, 2000000);
    
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
                    Features.Profil(currentUser);
                    break;
                case "2":
                    Features.Deposit(currentUser);
                    break;
                case "3":
                    Features.Withdraw(currentUser);
                    break;
                case "4":
                    Features.Transfer(currentUser, allCustomers);
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
        Console.WriteLine("Menu:\n1. Profile\n2. Deposit\n3. Withdraw \n4. Transfer");
        Console.WriteLine("5. Print Mutation\n6. End Month Process\n0. Switch User");
        Console.Write("Select an option: ");
    }

    private static string? GetOption()
    {
        return Console.ReadLine();
    }
}