using System.Diagnostics.CodeAnalysis;
using Bank.Abstract;
using Bank.AccountType;
using Bank.Class;

namespace Bank;

class Program
{
    static Customer c1 = new Customer("Abiem");
    static Customer c2 = new Customer("Fariz");
    
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
        Console.WriteLine("4. Print Mutation\n5. End Month Process\n6. Add Account\n0. Switch User");
        Console.Write("Select an option: ");
    }

    private static string? GetOption()
    {
        return Console.ReadLine();
    }
}