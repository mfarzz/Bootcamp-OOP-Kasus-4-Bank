using PracticeBank.AccountType;
using PracticeBank.Class;
using PracticeBank.Feature;

namespace PracticeBank;

class Program
{
    static void Main(string[] args)
    {
        var bank = new Bank();
        SeedData(bank);

        while (true)
        {
            Console.Clear();
            var currentUser = PickUser(bank);

            while (true)
            {
                Console.Clear();
                ShowMenu();
                var option = Console.ReadLine();
                Console.WriteLine("=============================");

                switch (option)
                {
                    case "1":
                        Features.Profil(currentUser);
                        break;
                    case "2":
                        Features.Deposit(bank, currentUser);
                        break;
                    case "3":
                        Features.Withdraw(bank, currentUser);
                        break;
                    case "4":
                        Features.Transfer(bank, currentUser);
                        break;
                    case "5":
                        Features.PrintMutation(currentUser);
                        break;
                    case "6":
                        Features.EndMonthProcess(bank);
                        break;
                    case "7":
                        Features.AddAccount(bank, currentUser);
                        break;
                    case "8":
                        Features.ToggleBlock(bank);
                        break;
                    case "9":
                        Features.ShowLargeTransactions(bank);
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

                if (option == "0") break;

                Console.WriteLine("===== Enter to continue =====");
                Console.ReadLine();
            }
        }
    }

    private static void SeedData(Bank bank)
    {
        var c1 = new Customer("Abiem", "CUST01");
        var c2 = new Customer("Fariz", "CUST02");
        bank.AddCustomer(c1);
        bank.AddCustomer(c2);

        bank.OpenAccount(c1, new Saving("sa-01", c1.Name, 200000));
        bank.OpenAccount(c1, new Checking("ca-01", c1.Name, 1500000));
        bank.OpenAccount(c1, new TimeDeposit("tda-01", c1.Name, 10000000, 2));

        bank.OpenAccount(c2, new Saving("sa-02", c2.Name, 150000));
        bank.OpenAccount(c2, new Checking("ca-02", c2.Name, 800000));
        bank.OpenAccount(c2, new TimeDeposit("tda-02", c2.Name, 2000000, 3));
    }

    private static Customer PickUser(Bank bank)
    {
        while (true)
        {
            Console.WriteLine("Registered Customer:");
            for (var i = 0; i < bank.Customers.Count; i++)
                Console.WriteLine($"{i + 1}. {bank.Customers[i].Name}");
            
            Console.Write("Pick user: ");
            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                if (choice >= 1 && choice <= bank.Customers.Count) return bank.Customers[choice - 1];
            }

            Console.WriteLine("Pilihan tidak valid.\n");
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Menu:\n1. Profile\n2. Deposit\n3. Withdraw\n4. Transfer");
        Console.WriteLine("5. Print Mutation\n6. End Month Process (semua rekening)\n7. Add Account");
        Console.WriteLine("8. Blokir / Buka Blokir Rekening\n9. Transaksi Besar\n0. Switch User\nx. Exit");
        Console.Write("Select an option: ");
    }
}