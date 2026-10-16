using PracticeBank.Abstract;
using PracticeBank.AccountType;
using PracticeBank.Class;
using PracticeBank.Feature;

namespace PracticeBank;

class Program
{
    static Customer c1 = new Customer("Abiem", "CUST01");
    static Customer c2 = new Customer("Fariz", "CUST02");
    static Customer[] allCustomers = { c1, c2 };
    
    private static Saving sa1 = new Saving("sa-01", c1.Name, 200000);
    private static Checking ca1 = new Checking("ca-01", c1.Name, 1500000);
    private static TimeDeposit tda1 = new TimeDeposit("tda-01", c1.Name, 10000000, 2);
    
    private static Saving sa2 = new Saving("sa-02", c2.Name, 150000);
    private static Checking ca2 = new Checking("ca-02", c2.Name, 800000);
    private static TimeDeposit tda2 = new TimeDeposit("tda-02", c2.Name, 2000000, 3);
    
    static Account[] accountsC1 = { sa1, ca1, tda1 };
    static Account[] accountsC2 = { sa2, ca2, tda2 };
    
    static Bank bank = new Bank(allCustomers, accountsC1, accountsC2);
    
    static void Main(string[] args)
    {
        foreach (var acc in accountsC1) c1.AddAccount(acc);
        foreach (var acc in accountsC2) c2.AddAccount(acc);
        
        // Coba atur saldo dari luar kelas
        // c1.Accounts[0].Balance = 20000;

        while (true)
        {
            Console.Clear();
            // Customer currentUser = PickUser();
            var currentUser = PickUser();
            var accounts = currentUser == c1 ? accountsC1 : accountsC2;
            while (true)
            {
                Console.Clear();
                ShowMenu();
                var option = GetOption();
                Console.WriteLine("=============================");
                switch (option)
                {
                    case "1":
                        Features.Profil(currentUser);
                        break;
                    case "2":
                        Features.Deposit(accounts);
                        break;
                    case "3":
                        Features.Withdraw(accounts);
                        break;
                    case "4":
                        Features.Transfer(accounts, bank);
                        break;
                    case "5":
                        Features.PrintMutation(accounts);
                        break;
                    case "6":
                        Features.EndMonthProcess(accounts);
                        break;
                    case "7":
                        Features.AddAccount(currentUser, accounts, bank);
                        break;
                    case "0":
                        break;
                    case "x":
                        Console.WriteLine("Thank you, have a nice day!");
                        return;
                    default:
                        Console.WriteLine("Opsi Invalid.");
                        break;
                }

                if (option == "0")
                {
                    break;
                }

                Console.WriteLine("===== Enter to continue =====");
                Console.ReadLine();
            }
        }
    }

    private static Customer PickUser()
    {
        while (true)
        {
            Console.WriteLine($"Registered Customer:\n1. {c1.Name}\n2. {c2.Name}\n3. Total Account");
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
        Console.WriteLine("5. Print Mutation\n6. End Month Process\n" + 
                          "7. Add Account\n0. Switch User\nx. Exit");
        Console.Write("Select an option: ");
    }

    private static string? GetOption()
    {
        return Console.ReadLine();
    }
}