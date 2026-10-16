using PracticeBank.Abstract;
using PracticeBank.AccountType;
using PracticeBank.Class;
using PracticeBank.Exceptions;

namespace PracticeBank.Feature;

public static class Features
{
    // Menu 1
    public static void Profil(Customer customer)
    {
        customer.PrintProfile();
    }

    // Menu 2
    public static void Deposit(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts);
        if (account == null) return;

        var amount = ReadAmount("Jumlah deposit: Rp ");
        if (amount == null) return;

        if (!TryRun(() => bank.Deposit(account, amount.Value))) return;

        Console.WriteLine($"Deposit berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 3
    public static void Withdraw(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts);
        if (account == null) return;

        var amount = ReadAmount("Jumlah penarikan: Rp ");
        if (amount == null) return;

        if (!TryRun(() => bank.Withdraw(account, amount.Value))) return;

        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 4
    public static void Transfer(Bank bank, Customer sender)
    {
        Account source;
        while (true)
        {
            var picked = PickAccount(sender.Accounts);
            if (picked == null) return;

            if (picked is ITransferable)
            {
                source = picked;
                break;
            }

            Console.WriteLine("Jenis rekening ini tidak mendukung metode transfer.");
            Console.WriteLine("Silakan pilih rekening lain.\n");
        }

        Account destination;
        while (true)
        {
            Console.Write("Nomor Rekening Tujuan (kosongkan untuk batal): ");
            var number = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(number)) return;

            var found = bank.FindAccount(number);
            if (found == null)
            {
                Console.WriteLine("Rekening tujuan tidak ditemukan.\n");
                continue;
            }

            if (ReferenceEquals(found, source))
            {
                Console.WriteLine("Rekening tujuan tidak boleh sama dengan rekening asal.\n");
                continue;
            }

            destination = found;
            break;
        }

        var amount = ReadAmount("Jumlah transfer: Rp ");
        if (amount == null) return;

        if (!TryRun(() => bank.TransferMoney(source, destination, amount.Value))) return;

        Console.WriteLine("=========== BUKTI TRANSFER ==========");
        Console.WriteLine(
            $"Transfer {Utils.FormatRupiah(amount.Value)} ke {destination.OwnerName} ({destination.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: {Utils.FormatRupiah(source.Balance)}");
    }

    // Menu 5
    public static void PrintMutation(Customer customer)
    {
        var account = PickAccount(customer.Accounts);
        account?.PrintStatement();
    }

    // Menu 6
    public static void EndMonthProcess(Bank bank)
    {
        bank.ProcessMonthEnd();
    }

    // Menu 7
    public static void AddAccount(Bank bank, Customer customer)
    {
        var type = PickAccountType();
        var number = GenerateAccountNumber(type, bank);

        var balance = ReadAmount("Saldo awal: Rp ");
        if (balance == null) return;
        if (balance.Value < 0)
        {
            Console.WriteLine("Saldo awal tidak boleh negatif.");
            return;
        }

        Account newAccount;
        switch (type)
        {
            case Abstract.AccountType.Saving:
                var saving = new Saving(number, customer.Name, balance.Value);
                if (balance.Value < saving.MinimumBalance)
                {
                    Console.WriteLine($"Saldo awal tabungan minimal {Utils.FormatRupiah(saving.MinimumBalance)}.");
                    return;
                }
                newAccount = saving;
                break;
            case Abstract.AccountType.Checking:
                newAccount = new Checking(number, customer.Name, balance.Value);
                break;
            default:
                var months = ReadAmount("Jangka waktu deposito (bulan): ");
                if (months == null) return;
                if (months.Value <= 0)
                {
                    Console.WriteLine("Jangka waktu harus lebih dari 0 bulan.");
                    return;
                }
                newAccount = new TimeDeposit(number, customer.Name, balance.Value, months.Value);
                break;
        }

        if (!bank.OpenAccount(customer, newAccount))
        {
            Console.WriteLine("Gagal membuat rekening baru.");
            return;
        }

        Console.WriteLine($"Rekening berhasil dibuat: {number} | Saldo: {Utils.FormatRupiah(newAccount.Balance)}");
    }

    // Menu 8: blokir / buka blokir (toggle)
    public static void ToggleBlock(Bank bank)
    {
        Console.WriteLine("Daftar rekening:");
        foreach (var acc in bank.Accounts)
            Console.WriteLine($"  {acc.AccountNumber} | {acc.OwnerName} | " +
                              (bank.IsBlocked(acc.AccountNumber) ? "DIBLOKIR" : "aktif"));

        Console.Write("Nomor rekening (kosongkan untuk batal): ");
        var number = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(number)) return;

        if (bank.FindAccount(number) == null)
        {
            Console.WriteLine("Rekening tidak ditemukan.");
            return;
        }

        if (bank.IsBlocked(number))
        {
            bank.UnblockAccount(number);
            Console.WriteLine($"Blokir rekening {number} dibuka.");
        }
        else
        {
            bank.BlockAccount(number);
            Console.WriteLine($"Rekening {number} diblokir.");
        }
    }

    // Menu 9
    public static void ShowLargeTransactions(Bank bank)
    {
        if (bank.LargeTransactions.Count == 0)
        {
            Console.WriteLine($"Belum ada transaksi besar (>= {Utils.FormatRupiah(Bank.LargeTransactionThreshold)}).");
            return;
        }

        Console.WriteLine("Transaksi besar:");
        foreach (var line in bank.LargeTransactions) Console.WriteLine($"  {line}");
    }

    private static bool TryRun(Action transaction)
    {
        try
        {
            transaction();
            return true;
        }
        catch (InsufficientBalanceException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (DepositLockedException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Input ditolak: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Transaksi gagal.");
        return false;
    }

    private static Account? PickAccount(IReadOnlyList<Account> accounts)
    {
        if (accounts.Count == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");
        for (var i = 0; i < accounts.Count; i++)
            Console.WriteLine($"    {i + 1}. {accounts[i].AccountNumber} | {Utils.FormatRupiah(accounts[i].Balance)}");

        Console.Write("Nomor pilihan: ");
        if (int.TryParse(Console.ReadLine(), out var index) && index >= 1 && index <= accounts.Count)
            return accounts[index - 1];

        Console.WriteLine("Pilihan rekening tidak valid.");
        return null;
    }

    private static int? ReadAmount(string prompt)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var amount)) return amount;

        Console.WriteLine("Input tidak valid. Harus berupa bilangan bulat.");
        return null;
    }

    private static Abstract.AccountType PickAccountType()
    {
        while (true)
        {
            Console.WriteLine("Pilih jenis rekening:");
            Console.WriteLine("1. Tabungan\n2. Giro\n3. Deposito");
            Console.Write("Pilihan: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return Abstract.AccountType.Saving;
                case "2":
                    return Abstract.AccountType.Checking;
                case "3":
                    return Abstract.AccountType.TimeDeposit;
                default:
                    Console.WriteLine("Input invalid.\n");
                    break;
            }
        }
    }

    private static string GenerateAccountNumber(Abstract.AccountType type, Bank bank)
    {
        var prefix = type switch
        {
            Abstract.AccountType.Saving => "sa-",
            Abstract.AccountType.Checking => "ca-",
            _ => "tda-"
        };

        while (true)
        {
            var number = $"{prefix}{Random.Shared.Next(1, 1000):D3}";
            if (bank.FindAccount(number) == null) return number;
        }
    }
}