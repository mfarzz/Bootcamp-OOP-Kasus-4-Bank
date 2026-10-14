using System.Text;
using Bank.Abstract;
using Bank.AccountType;
using Bank.Class;

namespace Bank;

public class Features2
{
    // Console.WriteLine("4. Print Mutation\n5. End Month Process\n6. Add Account\n0. Switch User");

    public static void PrintMutation(Customer customer)
    {
        Account acc = PickAccount(customer);
        acc.PrintHistory();
    }

    public static void EndMonthProcess(Customer customer)
    {
        var accounts =  customer.Accounts;
        foreach (var acc in accounts)
        {
            acc.ApplyEndMonth();
        }
    }

    public static void AddAccount(Customer customer)
    {
        var accountType = PickAccountType();
        var accNumber = GenerateNumber(5);
        var initialBalance = GetBalance();

        Account acc;
        
        switch (accountType)
        {
            case Abstract.AccountType.Saving:
                acc = new SavingAccount(accNumber, customer, initialBalance);
                break;
            case Abstract.AccountType.Checking:
                acc = new CheckingAccount(accNumber, customer, initialBalance);
                break;
            case Abstract.AccountType.TimeDeposit:
                acc = new TimeDepositAccount(accNumber, customer, initialBalance);
                break;
            default:
                Console.WriteLine("Invalid input");
                return;
        }

        customer.AddAccount(acc);
    }

    private static int GetBalance()
    {
        while (true)
        {
            Console.Write("Initial balance: ");
            var input = Console.ReadLine();
            try
            {
                return Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Input valid balance!");
            }
        }
    }

    private static Abstract.AccountType PickAccountType()
    {
        while (true)
        {
            Console.WriteLine("Choose an account type:");
            Console.WriteLine("1. Saving\n2. Checking\n3. Deposit");
            Console.WriteLine("Your choice: ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    return Abstract.AccountType.Saving;
                case "2":
                    return Abstract.AccountType.Checking;
                case "3":
                    return Abstract.AccountType.TimeDeposit;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

    }
    
    private static Account PickAccount(Customer customer)
    {
        var accounts = customer.Accounts;
        while (true)
        {
            Console.WriteLine($"Registered Account:");
            for (var i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {customer.Accounts[i].AccountNumber} | {customer.Accounts[i].AccountNumber}");
            }
            Console.Write("Pick account: ");
            var  input = Console.ReadLine();
            var pickIndex = -1;
            try
            {
                pickIndex = Convert.ToInt32(input) - 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid input; Pick between 1 - {accounts.Length}");
                continue;
            }
            if (pickIndex >= 0 && pickIndex < accounts.Length) return accounts[pickIndex];
        }
    }

    private static string GenerateNumber(int length)
    {
        Random random = new Random();
        StringBuilder result = new StringBuilder(length);
        
        for (int i = 0; i < length; i++)
        {
            // Generate random 1-9 digit
            int digit = random.Next(0, 10);
            result.Append(digit);
        }
        
        return result.ToString();
    }
}