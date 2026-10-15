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

        double? amount = ReadAmount("Jumlah deposit: Rp");
        if (amount == null) return;

        if (!account.Deposit(amount.Value))
        {
            Console.WriteLine("Deposit gagal.");
            return;
        }
        Console.WriteLine($"Deposit berhasil. Saldo sekarang: Rp{account.GetBalance():N0}");
    }

    // Menu 3
    public void Withdraw(Customer customer)
    {
        Account? account = PickAccount(customer);
        if (account == null) return;

        double? amount = ReadAmount("Jumlah penarikan: Rp");
        if (amount == null) return;

        // return bisa diganti string pesan error
        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal. Saldo tidak mencukupi atau syarat rekening belum terpenuhi.");
            return;
        }
        
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: Rp{account.GetBalance():N0}");
    }
    
    //Menu 4
    public void Transfer(Customer sender, Customer[] allCustomers)
    {
        Account? source = PickAccount(sender);
        if (source == null) return;

        if (source is not ITransferable transferable)
        {
            Console.WriteLine("Jenis rekening ini tidak mendukung metode transafer");
            return;
        }

        Console.Write("Nomor Rekening Tujuan:   ");
        string? destNumber = Console.ReadLine();
        
        var found = FindAccount(destNumber, allCustomers);
        if (found == null)
        {
            Console.WriteLine("Rekening tujuan tidak ditemukan");
            return;
        }

        (Account destination, Customer owner) = found.Value;

        if (destination == source)
        {
            Console.WriteLine("Rekening tujuan tidak boleh sama dengan rekening asal");
            return;
        }
        
        double? ammount = ReadAmount("Jumlah transefer: Rp");
        if(ammount == null) return;

        if (!transferable.TransferTo(destination, ammount.Value))
        {
            Console.WriteLine("Transfer gagal. Saldo tidak mencukupi atau batas rekening terlampaui");
            return;
        } 
        
        Console.WriteLine($"Transfer Rp{ammount.Value:N} ke {owner.Name} ({destination.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: Rp{source.GetBalance():N}");
    }
    
    // Menu 5
    public void PrintMutation(Customer customer)
    {
        Account? acc = PickAccount2(customer);
        if (acc == null)
        {
            Console.WriteLine("Akun tidak ditemukan.");
            return;
        }
        acc.PrintHistory();
    }
    
    // Menu 6
    public void EndMonthProcess(Customer customer)
    {
        var accounts =  customer.Accounts;
        foreach (var acc in accounts)
        {
            acc?.ApplyEndMonth();
        }
    }
    
    // Menu 7
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
        
        Console.WriteLine($"Akun berhasil dibuat. Saldo : {initialBalance:N}");
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
            Console.WriteLine($"    {i + 1}. {acc.AccountNumber} | Rp{acc.GetBalance():N}");
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
    
    private (Account, Customer)? FindAccount(string? accountNumber, Customer[] customers)
    {
        if (string.IsNullOrWhiteSpace(accountNumber)) return null;

        foreach (Customer customer in customers)
        {
            for (int i = 0; i < customer.AccountCount; i++)
            {
                if (customer.Accounts[i].AccountNumber == accountNumber.Trim())
                {
                    return (customer.Accounts[i], customer);
                }
            }
        }
        return null;
    }
    
    private Account? PickAccount2(Customer customer)
    {
        var accounts = customer.Accounts;
        while (true)
        {
            Console.WriteLine($"Rekening yang terdaftar: ");
            for (var i = 0; i < customer.AccountCount; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].AccountNumber} | {accounts[i].AccountNumber} | {accounts[i].GetBalance():N}");
            }
            Console.Write("Pilihan: ");
            var  input = Console.ReadLine();
            var pickIndex = -1;
            try
            {
                pickIndex = Convert.ToInt32(input) - 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Input invalid; Pilihan 1 - {customer.AccountCount}.");
                continue;
            }
            if (pickIndex >= 0 && pickIndex < accounts.Length) return accounts[pickIndex];
        }
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