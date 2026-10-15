using Bank.Abstract;
using Bank.Class;

namespace Bank.Feature;

public class Features
{
    // Menu 1
    public static void Profil(Customer customer)
    {
        customer.PrintProfile();
    }

    // Menu 2
    public static void Deposit(Customer customer)
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
    public static void Withdraw(Customer customer)
    {
        Account? account = PickAccount(customer);
        if (account == null) return;

        double? amount = ReadAmount("Jumlah penarikan: Rp");
        if (amount == null) return;

        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal. Saldo tidak mencukupi atau syarat rekening belum terpenuhi.");
            return;
        }
        
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: Rp{account.GetBalance():N0}");
    }

    //Menu 4
    public static void Transfer(Customer sender, Customer[] allCustomers)
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

    private static Account? PickAccount(Customer customer)
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
            Console.WriteLine($"    {i + 1}. {acc.AccountNumber} | Rp{acc.GetBalance():N0}");
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

    private static double? ReadAmount(string input)
    {
        Console.Write(input);
        if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
        {
            return amount;
        }

        Console.WriteLine("Jumlah tidak valid. Harus berupa angka lebih dari 0.");
        return null;
    }
    
    private static (Account, Customer)? FindAccount(string? accountNumber, Customer[] customers)
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
}