using System.Text;
using Bank.Abstract;
using Bank.AccountType;
using Bank.Class;

namespace Bank.Feature;

public class Features
{
    // Menu 1
    public void Profil(Customer customer)
    {
        customer.PrintProfile();
    }

    // Menu 2
    public void Deposit(Customer customer)
    {
        Account? account = PickAccount(customer);
        if (account == null) return;

        double? amount = ReadAmount("Jumlah deposit: Rp ");
        if (amount == null) return;

        if (!account.Deposit(amount.Value))
        {
            Console.WriteLine("Deposit gagal.");
            return;
        }
        Console.WriteLine($"Deposit berhasil. Saldo sekarang: Rp. {account.GetBalance():N}");
    }

    // Menu 3
    public void Withdraw(Customer customer)
    {
        Account? account = PickAccount(customer);
        if (account == null) return;

        double? amount = ReadAmount("Jumlah penarikan: Rp ");
        if (amount == null) return;

        // return bisa diganti string pesan error
        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal.");
            return;
        }
        
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: Rp. {account.GetBalance():N0}");
    }
    
    // Menu 4
    public void PrintMutation(Customer customer)
    {
        Account? acc = PickAccount(customer);
        if (acc == null)
        {
            Console.WriteLine("Akun tidak ditemukan.");
            return;
        }
        acc.PrintHistory();
    }
    
    // Menu 5
    public void EndMonthProcess(Customer customer)
    {
        var accounts =  customer.Accounts;
        foreach (var acc in accounts)
        {
            acc?.ApplyEndMonth();
        }
    }
    
    // Menu 6
    public void AddAccount(Customer customer)
    {
        if (customer.AccountCount == 5)
        {
            Console.WriteLine($"Jumlah akun mencapai maksimal: {customer.AccountCount}");
            return;
        }
        var accountType = PickAccountType();
        var accNumber = GenerateNumber(5);
        var initialBalance = GetInputBalance();

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
                Console.WriteLine("Input Invalid");
                return;
        }

        if (!customer.AddAccount(acc))
        {
            Console.WriteLine("Gagal membuat akun baru.");
            return;
        }
        
        Console.WriteLine($"Akun berhasil dibuat. Saldo : Rp. {initialBalance:N}");
    }
    
    private Account? PickAccount(Customer customer)
    {
        if (customer.AccountCount == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");
        for (int i = 0; i < customer.AccountCount; i++)
        {
            Account acc = customer.Accounts[i];
            Console.WriteLine($"    {i + 1}. {acc.AccountNumber} | Rp. {acc.GetBalance():N}");
        }

        Console.Write("Nomor rekening (urutan): ");
        if (int.TryParse(Console.ReadLine(), out int index)
            && index >= 1 && index <= customer.AccountCount)
        {
            return customer.Accounts[index - 1];
        }

        Console.WriteLine("Pilihan rekening tidak valid.");
        return null;
    }

    private double? ReadAmount(string input)
    {
        Console.Write(input);
        if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
        {
            return amount;
        }

        Console.WriteLine("Jumlah tidak valid. Harus berupa angka lebih dari 0.");
        return null;
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
            try
            {
                return Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Input invalid.");
            }
        }
    }
}