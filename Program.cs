using Bank.AccountType;
using Bank.Class;
using Bank.Exceptions;
using Bank.Feature;

namespace Bank;

class Program
{
    static void Main(string[] args)
    {
        Class.Bank bank = new Class.Bank();
        Features features = new Features(bank);

        // Coba atur saldo dari luar kelas
        // c1.Accounts[0].Balance = 20000;

        while (true)
        {
            Console.Clear();
            ShowMenu();
            var option = GetOption();
            Console.WriteLine("=============================");
            switch (option)
            {
                case "1":
                    // Cetak profil seluruh user
                    try
                    {
                        features.Profil();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }

                    break;
                case "2":
                    // deposit
                    try
                    {
                        features.Deposit();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid.");
                    }
                    catch (ArithmeticException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "3":
                    // withdraw
                    try
                    {
                        features.Withdraw();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (InsufficientBalanceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (DepositLockException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "4":
                    // transfer
                    try
                    {
                        features.Transfer();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (InsufficientBalanceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (DepositLockException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "5":
                    // print mutation
                    try
                    {
                        features.PrintMutation();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (InsufficientBalanceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (DepositLockException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "6":
                    // process end month
                    try
                    {
                        features.EndMonthProcess().Wait();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (InsufficientBalanceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (DepositLockException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "7":
                    // add account
                    try
                    {
                        features.AddAccount();
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input must be an integer.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Input is too big.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Input invalid");
                    }
                    catch (InsufficientBalanceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (DepositLockException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception: " + e.Message);
                    }
                    break;
                case "8":
                    features.PrintBalancePerType();
                    break;
                case "9":
                    features.PrintCustomerWithHighestAssets();
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