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

    static Savings sa1 = new Savings("sa-01", c1.Name, 20000000);
    static Checking ca1 = new Checking("ca-01", c1.Name, 150000000);
    static TimeDeposit tda1 = new TimeDeposit("tda-01", c1.Name, 1000000000, 2);

    static Savings sa2 = new Savings("sa-02", c2.Name, 15000000);
    static Checking ca2 = new Checking("ca-02", c2.Name, 80000000);
    static TimeDeposit tda2 = new TimeDeposit("tda-02", c2.Name, 200000000, 3);

    static Account[] allAccounts = { sa1, ca1, tda1, sa2, ca2, tda2 };

    static Bank bank = new Bank(allCustomers, allAccounts);

    static void Main(string[] args)
    {
        c1.AddAccount(sa1);
        c1.AddAccount(ca1);
        c1.AddAccount(tda1);

        c2.AddAccount(sa2);
        c2.AddAccount(ca2);
        c2.AddAccount(tda2);

        bank.RecordTransaction("Transfer", sa1, 10000000);

        while (true)
        {
            Console.Clear();
            var currentUser = PickUser();

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
                        Features.PrintMutation(bank, currentUser);
                        break;
                    case "6":
                        Features.EndMonthProcess(bank, currentUser);
                        break;
                    case "7":
                        Features.AddAccount(bank, currentUser);
                        break;
                    case "8":
                        Features.ToggleBlock(bank, currentUser);
                        break;
                    case "9":
                        Features.ShowLargeTransactions(bank, currentUser);
                        break;
                    case "10":
                        Features.ShowAverageBalance(currentUser);
                        break;
                    case "11":
                        Features.EndMonthProcessAsync(bank).Wait();
                        break;
                    case "12":
                        Features.TransferInterbankAsync(bank, currentUser).Wait();
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

    private static Customer PickUser()
    {
        while (true)
        {
            Console.WriteLine("Registered Customer:");
            for (var i = 0; i < bank.Customers.Count; i++)
                Console.WriteLine($"{i + 1}. {bank.Customers[i].Name}");
            Console.WriteLine($"{bank.Customers.Count + 1}. Total Account");

            Console.Write("Pick user: ");
            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                if (choice >= 1 && choice <= bank.Customers.Count) return bank.Customers[choice - 1];

                if (choice == bank.Customers.Count + 1)
                {
                    Console.WriteLine($"Total rekening terdaftar: {Account.TotalAccounts}\n");
                    continue;
                }
            }

            Console.WriteLine("Pilihan tidak valid.\n");
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Menu:\n1. Profile\n2. Deposit\n3. Withdraw\n4. Transfer");
        Console.WriteLine("5. Print Mutation\n6. End Month Process\n7. Add Account");
        Console.WriteLine("8. Blokir / Buka Blokir Rekening\n9. Transaksi Besar");
        Console.WriteLine("10. Rata rata saldo semua rekening\n11. End Mount Process Async All");
        Console.WriteLine("12. Transfer Antar Bank\n0. Switch User\nx. Exit");
        Console.Write("Select an option: ");
    }
}