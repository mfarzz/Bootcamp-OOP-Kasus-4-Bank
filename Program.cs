using Bank.AccountType;
using Bank.Class;
using Bank.Feature;

namespace Bank;

class Program
{
    static Features features = new Features();
    
    static Customer c1 = new Customer("Abiem", "CUST01");
    static Customer c2 = new Customer("Fariz", "CUST02");
    static Customer[] allCustomers = { c1, c2 };

    private static Saving sa1 = new Saving("sa-01", c1, 200000);
    private static Checking ca1 = new Checking("ca-01", c1, 1500000);
    private static TimeDeposit tda1 = new TimeDeposit("tda-01", c1, 10000000);
    
    private static Saving sa2 = new Saving("sa-02", c2, 150000);
    private static Checking ca2 = new Checking("ca-02", c2, 800000);
    private static TimeDeposit tda2 = new TimeDeposit("tda-02", c2, 2000000);
    
    static void Main(string[] args)
    {
        c1.AddAccount(sa1);
        c1.AddAccount(ca1);
        c1.AddAccount(tda1);
        
        c2.AddAccount(sa2);
        c2.AddAccount(ca2);
        c2.AddAccount(tda2);
        
        // Coba atur saldo dari luar kelas
        // c1.Accounts[0].Balance = 20000;

        while (true)
        {
            Console.Clear();
            Customer currentUser = PickUser();
            while (true)
            {
                Console.Clear();
                ShowMenu();
                var option = GetOption();
                Console.WriteLine("=============================");
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
                    case "4":
                        features.Transfer(currentUser, allCustomers);
                        break;
                    case "5":
                        features.PrintMutation(currentUser);
                        break;
                    case "6":
                        features.EndMonthProcess(currentUser);
                        break;
                    case "7":
                        features.AddAccount(currentUser);
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