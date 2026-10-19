using System.Text;
using Bank.Abstract;
using Bank.AccountType;
using Bank.Class;

namespace Bank.Feature;

public class Features
{
    private Class.Bank _bank;

    public Features(Class.Bank bank)
    {
        _bank = bank;
    }

    // Menu 1
    public void Profil()
    {
        var customers =  _bank.GetAllCustomers();
        foreach (var customer in customers)
        {
            customer.PrintProfile();
            Console.WriteLine("==================================");
        }
        // var customer = PickCustomer();
        // customer.PrintProfile();
    }

    // Menu 2
    public void Deposit()
    {
        var customer = PickCustomer();
        var account = PickAccount(customer);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        int amount = ReadAmount("Jumlah deposit: Rp ");

        if (!account.Deposit(amount))
        {
            Console.WriteLine("Deposit gagal.");
            return;
        }

        Console.WriteLine($"Deposit berhasil. Saldo sekarang: Rp. {account.Balance:N}");
    }

    // Menu 3
    public void Withdraw()
    {
        var customer = PickCustomer();
        var account = PickAccount(customer);
        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }
        int amount = ReadAmount("Jumlah penarikan: Rp ");
    
        if (!account.Withdraw(amount))
        {
            Console.WriteLine("Penarikan gagal.");
            return;
        }
        
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: Rp. {account.Balance:N}");
    }

    //Menu 4
    public void Transfer()
    {
        var sourceCustomer = PickCustomer();
        var sourceAccount = PickAccount(sourceCustomer);

        if (sourceAccount == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }
        
        if (sourceAccount is not ITransferable transferable)
        {
            Console.WriteLine("Account is not able to transfer.");
            return;
        }
        
        var destCustomer = PickCustomer();
        var destAccount = PickAccount(destCustomer);

        if (destAccount == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }
        
        if (sourceAccount == destAccount)
        {
            Console.WriteLine("Cannot transfer to the same account.");
            return;
        }
        
        int amount = ReadAmount("Jumlah transefer: Rp ");

        transferable.Transfer(destAccount, amount);
    
        Console.WriteLine("===========BUKTI TRANSFER==========");
        Console.WriteLine($"Transfer Rp{amount:N} ke {destCustomer.Name} ({destAccount.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: Rp{sourceAccount.Balance:N}");
    }

    // Menu 5
    public void PrintMutation()
    {
        var customer = PickCustomer();
        Account? acc = PickAccount(customer);
        if (acc == null)
        {
            Console.WriteLine("Akun tidak ditemukan.");
            return;
        }
        acc.PrintStatement();
    }

    // Menu 6
    public void EndMonthProcess()
    {
        var accounts = _bank.GetAllAccounts();
        foreach (var acc in accounts)
        {
            acc.ProcessMonthEnd();
        }
    }

    // // Menu 7
    public void AddAccount()
    {
        var customer = PickCustomer();
        
        var accountType = PickAccountType();
        var accNumber = GenerateNumber(5);
        var initialBalance = GetInputBalance();
    
        Account acc;
        
        switch (accountType)
        {
            case Abstract.AccountType.Saving:
                acc = new Saving(accNumber, customer.Name, initialBalance);
                break;
            case Abstract.AccountType.Checking:
                acc = new Checking(accNumber, customer.Name, initialBalance);
                break;
            case Abstract.AccountType.TimeDeposit:
                acc = new TimeDeposit(accNumber, customer.Name, initialBalance, 6);
                break;
            default:
                Console.WriteLine("Input Invalid");
                return;
        }
    
        if (!customer.AddAccount(acc))
        {
            Console.WriteLine("Gagal membuat akun baru.");
            return;
        }
        
        Console.WriteLine($"Akun berhasil dibuat. Saldo : {initialBalance:N}");
    }
    
    // Menu 8
    public void BlockAccount()
    {
        
    }

    private Account? PickAccount(Customer customer)
    {
        var accounts =  customer.GetAccounts();
        if (accounts.Count == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");

        var i = 1;
        foreach (var acc in accounts)
        {
            Console.WriteLine($"    {i}. {acc.AccountNumber} | Rp. {acc.Balance:N}");
            i++;
        }

        Console.Write("Nomor rekening: ");
        var input = Console.ReadLine();

        Account? account = accounts.Find(acc => acc.AccountNumber == input);

        return account;
    }

    /**
     * Exception thrown:
     * FormatException, OverflowException, ArithMeticException
     */
    private int ReadAmount(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        var result = Convert.ToInt32(input);
        if (result <= 0)
        {
            throw new ArithmeticException("Input must be greater than zero.");
        }

        return result;
    }

    private void PrintAllAccount()
    {
        
    }

    public void PrintBalancePerType()
    {
        _bank.BalancePerAccountType();
    }

    public void PrintCustomerWithHighestAssets()
    {
        var c = _bank.CustomerWithHighestAssets();
        if (c == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.WriteLine($"Customer with highest assets\n{c.Name} | Total assets: {c.TotalAssets}");
    }

    private Abstract.AccountType PickAccountType()
    {
        while (true)
        {
            Console.WriteLine("Pilih jenis rekening: ");
            Console.WriteLine("1. Tabungan\n2. Giro\n3. Deposit");
            Console.Write("Pilihan : ");
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
                    Console.WriteLine("Input invalid.");
                    break;
            }
        }
    }

    private string GenerateNumber(int length)
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

    private int GetInputBalance()
    {
        while (true)
        {
            Console.Write("Saldo awal: ");
            var input = Console.ReadLine();
            return Convert.ToInt32(input);
        }
    }

    /**
     * Exception thrown:
     * Format, Overflow, ArgumentOutOfRange
     */
    private Customer PickCustomer()
    {
        var customers = _bank.GetAllCustomers();
        while (true)
        {
            Console.WriteLine("Registered Customer:");
            var i = 1;
            foreach (var customer in customers)
            {
                Console.WriteLine($"{i}. {customer.Name}");
                i++;
            }

            Console.Write("Pick user: ");

            var input = Console.ReadLine();
            var index = Convert.ToInt32(input) - 1;
            return customers[index];
        }
    }
}