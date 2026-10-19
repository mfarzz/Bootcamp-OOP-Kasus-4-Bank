using PracticeBank.Abstract;
using PracticeBank.AccountType;
using PracticeBank.Class;
using PracticeBank.Exceptions;

namespace PracticeBank.Feature;

public static class Features
{
    private static readonly TimeSpan InterbankTimeout = TimeSpan.FromSeconds(3);
    
    // Menu 1
    public static void Profil(Customer customer)
    {
        customer.PrintProfile();
    }

    // Menu 2
    public static void Deposit(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts, bank);
        if (account == null) return;

        var amount = ReadAmount("Jumlah deposit: Rp ");
        if (amount == null) return;

        var ok = TryRun(() =>
        {
            bank.EnsureNotBlocked(account);
            return account.Deposit(amount.Value);
        });
        if (!ok) return;

        bank.RecordTransaction("Deposit", account, amount.Value);
        Console.WriteLine($"Deposit berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 3
    public static void Withdraw(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts, bank);
        if (account == null) return;

        var amount = ReadAmount("Jumlah penarikan: Rp ");
        if (amount == null) return;

        var ok = TryRun(() =>
        {
            bank.EnsureNotBlocked(account);
            return account.Withdraw(amount.Value);
        });
        if (!ok) return;

        bank.RecordTransaction("Withdraw", account, amount.Value);
        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 4
    public static void Transfer(Bank bank, Customer sender)
    {
        // Rekening asal harus mendukung ITransferable (Savings / Checking)
        Account source;
        ITransferable transferable;
        while (true)
        {
            var picked = PickAccount(sender.Accounts, bank);
            if (picked == null) return;

            if (picked is ITransferable t)
            {
                source = picked;
                transferable = t;
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

        // Transfer dijalankan langsung oleh rekening asal (Savings / Checking)
        var ok = TryRun(() =>
        {
            bank.EnsureNotBlocked(source);
            bank.EnsureNotBlocked(destination);
            return transferable.Transfer(destination, amount.Value);
        });
        if (!ok) return;

        bank.RecordTransaction("Transfer", source, amount.Value);
        Console.WriteLine("=========== BUKTI TRANSFER ==========");
        Console.WriteLine(
            $"Transfer {Utils.FormatRupiah(amount.Value)} ke {destination.OwnerName} ({destination.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: {Utils.FormatRupiah(source.Balance)}");
    }

    // Menu 5
    public static void PrintMutation(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts, bank);
        account?.PrintStatement();
    }

    // Menu 6: hanya rekening milik nasabah yang dipilih
    public static void EndMonthProcess(Bank bank, Customer customer)
    {
        if (customer.AccountCount == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return;
        }

        bank.ProcessMonthEnd(customer);
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

        if (type == Abstract.AccountType.Savings && balance.Value < Savings.DefaultMinimumBalance)
        {
            Console.WriteLine($"Saldo awal tabungan minimal {Utils.FormatRupiah(Savings.DefaultMinimumBalance)}.");
            return;
        }

        var months = 0;
        if (type == Abstract.AccountType.TimeDeposit)
        {
            var input = ReadAmount("Jangka waktu deposito (bulan): ");
            if (input == null) return;
            if (input.Value <= 0)
            {
                Console.WriteLine("Jangka waktu harus lebih dari 0 bulan.");
                return;
            }

            months = input.Value;
        }

        Account newAccount = type switch
        {
            Abstract.AccountType.Savings => new Savings(number, customer.Name, balance.Value),
            Abstract.AccountType.Checking => new Checking(number, customer.Name, balance.Value),
            _ => new TimeDeposit(number, customer.Name, balance.Value, months)
        };

        if (!bank.OpenAccount(customer, newAccount))
        {
            Console.WriteLine("Gagal membuat rekening baru.");
            return;
        }

        Console.WriteLine($"Rekening berhasil dibuat: {number} | Saldo: {Utils.FormatRupiah(newAccount.Balance)}");
    }

    // Menu 8: blokir / buka blokir (toggle), hanya rekening milik nasabah yang dipilih
    public static void ToggleBlock(Bank bank, Customer customer)
    {
        var account = PickAccount(customer.Accounts, bank);
        if (account == null) return;

        if (bank.IsBlocked(account.AccountNumber))
        {
            bank.UnblockAccount(account.AccountNumber);
            Console.WriteLine($"Blokir rekening {account.AccountNumber} dibuka.");
        }
        else
        {
            bank.BlockAccount(account.AccountNumber);
            Console.WriteLine($"Rekening {account.AccountNumber} diblokir.");
        }
    }

    // Menu 9: transaksi besar pada rekening milik nasabah yang dipilih diatas 5 juta
    public static void ShowLargeTransactions(Bank bank, Customer customer)
    {
        var records = bank.GetLargeTransactions(customer);
        if (records.Count == 0)
        {
            Console.WriteLine($"Belum ada transaksi besar (>= {Utils.FormatRupiah(Bank.LargeTransactionThreshold)}).");
            return;
        }

        Console.WriteLine($"Transaksi di atas {Utils.FormatRupiah(Bank.LargeTransactionThreshold)} milik {customer.Name}:");
        foreach (var r in records)
            Console.WriteLine($"  {r.Time:dd/MM/yyyy HH:mm} | {r.Type} | {r.AccountNumber} | {Utils.FormatRupiah(r.Amount)}");
    }
    
    // Menu 10: Rata rata saldo rekening
    public static void ShowAverageBalance(Customer customer)
    {
        if (customer.AccountCount == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return;
        }
        Console.WriteLine($"Rata rata saldo rekening {customer.Name} ({customer.AccountCount}) rekening: " 
            + Utils.FormatRupiah((int)Math.Round(customer.AverageBalance())));
    }

    // Menu 11: Untuk semua nasabah
    public static async Task EndMonthProcessAsync(Bank bank)
    {
        if (bank.Accounts.Count == 0)
        {
            Console.WriteLine("Tidak ada rekening terdaftar saat ini.");
            return;
        }

        await bank.ProcessMonthEndAsync();
    }
    
    // Menu 12: Transfer antar bank
    public static async Task TransferInterbankAsync(Bank bank, Customer customer)
    {
        Account source;
        ITransferable transferable;
        while (true)
        {
            var picked =  PickAccount(customer.Accounts, bank);
            if (picked == null) return;

            if (picked is ITransferable t)
            {
                source = picked;
                transferable = t;
                break;
            }

            Console.WriteLine("Jenis rekening ini tidak mendukung metode transfer");
            Console.WriteLine("Silahkan pilih rekening yang lain \n");
        }

        Console.Write("Nama bank tujuan: ");
        var bankName = Console.ReadLine()?.Trim();
        Console.Write("Nomor rekening tujuan: ");
        var destinationNumber = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(bankName) || string.IsNullOrEmpty(destinationNumber))
        {
            Console.WriteLine("Nama bank dan nomor rekening tujuan wajib diisi");
        }

        var amount = ReadAmount("Jumlah transfer: Rp ");
        if (amount == null) return;
        
        using var cts = new CancellationTokenSource(InterbankTimeout);
        Console.WriteLine($"Memproses transfer ke {bankName} (batas waktu {InterbankTimeout.TotalSeconds:0} detik)...");

        var ok = await TryRunAsync(() =>
        {
            bank.EnsureNotBlocked(source);
            return transferable.TransferInterbankAsync(bankName, destinationNumber, amount.Value, cts.Token);
        });
        
        if(!ok) return;
        
        bank.RecordTransaction("Transfer Antar Bank", source, amount.Value);
        Console.WriteLine("=========== BUKTI TRANSFER ANTAR BANK ==========");
        Console.WriteLine($"Transfer {Utils.FormatRupiah(amount.Value)} ke {bankName} ({destinationNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: {Utils.FormatRupiah(source.Balance)}");
    }
    
    // Menjalankan transaksi dan menerjemahkan exception menjadi pesan untuk pengguna
    private static bool TryRun(Func<bool> transaction)
    {
        try
        {
            if (transaction()) return true;
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

        Console.WriteLine("Transaksi gagal.");
        return false;
    }
    
    private static async Task<bool> TryRunAsync(Func<Task<bool>> transaction)
    {
        try
        {
            if (await transaction()) return true;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Transfer dibatalkan: melewati batas waktu. Saldo tidak berubah.");
            return false;
        }
        catch (InsufficientBalanceException ex)
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
 
        Console.WriteLine("Transaksi gagal.");
        return false;
    }


    private static Account? PickAccount(IReadOnlyList<Account> accounts, Bank bank)
    {
        if (accounts.Count == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");
        for (var i = 0; i < accounts.Count; i++)
            Console.WriteLine($"    {i + 1}. {accounts[i].AccountNumber} | {Utils.FormatRupiah(accounts[i].Balance)}" +
                              (bank.IsBlocked(accounts[i].AccountNumber) ? " | DIBLOKIR" : ""));

        Console.Write("Nomor pilihan: ");
        if (int.TryParse(Console.ReadLine(), out var index) && index >= 1 && index <= accounts.Count)
            return accounts[index - 1];

        Console.WriteLine("Pilihan rekening tidak valid.");
        return null;
    }

    // Hanya memastikan input berupa bilangan bulat; nilai 0 / negatif sengaja diteruskan
    // agar ditolak oleh ArgumentException dari Account.
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
                    return Abstract.AccountType.Savings;
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
            Abstract.AccountType.Savings => "sa-",
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