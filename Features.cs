using Bank.Abstract;
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

        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal. Saldo tidak mencukupi atau syarat rekening belum terpenuhi.");
            return;
        }
        
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: Rp{account.GetBalance():N0}");
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
}