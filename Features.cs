using PracticeBank.Abstract;
using PracticeBank.AccountType;
using PracticeBank.Class;

namespace PracticeBank.Feature;

public static class Features
{
    // Sama dengan batas rekening per nasabah di Customer
    private const int MaxAccounts = 3;

    // Menu 1
    public static void Profil(Customer customer)
    {
        customer.PrintProfile();
    }

    // Menu 2
    public static void Deposit(Account[] accounts)
    {
        var account = PickAccount(accounts);
        if (account == null) return;

        var amount = ReadAmount("Jumlah deposit: Rp ");
        if (amount == null) return;

        if (!account.Deposit(amount.Value))
        {
            Console.WriteLine("Deposit gagal.");
            return;
        }

        Console.WriteLine($"Deposit berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 3
    public static void Withdraw(Account[] accounts)
    {
        var account = PickAccount(accounts);
        if (account == null) return;

        var amount = ReadAmount("Jumlah penarikan: Rp ");
        if (amount == null) return;

        // Alasan gagal sudah dicetak oleh Withdraw() di masing-masing jenis rekening
        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal.");
            return;
        }

        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    // Menu 4
    public static void Transfer(Account[] senderAccounts, Bank bank)
    {
        Account source;
        ITransferable transferable;

        while (true)
        {
            var picked = PickAccount(senderAccounts);
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
            var destNumber = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(destNumber)) return;

            var found = bank.FindAccount(destNumber);
            if (found == null)
            {
                Console.WriteLine("Rekening tujuan tidak ditemukan.");
                Console.WriteLine("Silakan inputkan kembali.\n");
                continue;
            }

            if (ReferenceEquals(found, source))
            {
                Console.WriteLine("Rekening tujuan tidak boleh sama dengan rekening asal.");
                Console.WriteLine("Silakan inputkan kembali.\n");
                continue;
            }

            destination = found;
            break;
        }

        var amount = ReadAmount("Jumlah transfer: Rp ");
        if (amount == null) return;

        if (!transferable.Transfer(destination, amount.Value))
        {
            Console.WriteLine("Transfer gagal.");
            return;
        }

        Console.WriteLine("=========== BUKTI TRANSFER ==========");
        Console.WriteLine(
            $"Transfer {Utils.FormatRupiah(amount.Value)} ke {destination.OwnerName} ({destination.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: {Utils.FormatRupiah(source.Balance)}");
    }

    // Menu 5
    public static void PrintMutation(Account[] accounts)
    {
        var acc = PickAccount(accounts);
        if (acc == null) return;

        acc.PrintStatement();
    }

    // Menu 6
    public static void EndMonthProcess(Account[] accounts)
    {
        foreach (var acc in accounts) acc.ProcessMonthEnd();
    }

    // Menu 7
    public static Account[] AddAccount(Customer customer, Account[] accounts, Bank bank)
    {
        if (accounts.Length >= MaxAccounts)
        {
            Console.WriteLine($"Jumlah rekening mencapai maksimal: {MaxAccounts}");
            return accounts;
        }

        var type = PickAccountType();
        var number = GenerateAccountNumber(type, bank);

        var balance = ReadInitialBalance();
        if (balance == null) return accounts;

        Account newAccount;
        switch (type)
        {
            case Abstract.AccountType.Saving:
                newAccount = new Saving(number, customer.Name, balance.Value);
                break;
            case Abstract.AccountType.Checking:
                newAccount = new Checking(number, customer.Name, balance.Value);
                break;
            default:
                var months = ReadPositiveInt("Jangka waktu deposit (bulan): ");
                if (months == null) return accounts;
                newAccount = new TimeDeposit(number, customer.Name, balance.Value, months.Value);
                break;
        }

        if (newAccount is Saving saving && balance.Value < saving.MinimumBalance)
        {
            Console.WriteLine($"Saldo awal tabungan minimal {Utils.FormatRupiah(saving.MinimumBalance)}.");
            return accounts;
        }

        if (!customer.AddAccount(newAccount))
        {
            Console.WriteLine("Gagal membuat rekening baru.");
            return accounts;
        }

        var result = new Account[accounts.Length + 1];
        for (var i = 0; i < accounts.Length; i++) result[i] = accounts[i];
        result[accounts.Length] = newAccount;

        Console.WriteLine($"Rekening berhasil dibuat: {newAccount.AccountNumber} | " +
                          $"Saldo: {Utils.FormatRupiah(newAccount.Balance)}");
        return result;
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

    private static int? ReadInitialBalance()
    {
        Console.Write("Saldo awal: Rp ");
        if (int.TryParse(Console.ReadLine(), out var balance) && balance >= 0) return balance;

        Console.WriteLine("Saldo awal tidak valid. Harus berupa bilangan bulat, minimal 0.");
        return null;
    }

    private static int? ReadPositiveInt(string prompt)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value) && value > 0) return value;

        Console.WriteLine("Input tidak valid. Harus berupa bilangan bulat lebih dari 0.");
        return null;
    }

    private static Account? PickAccount(Account[] accounts)
    {
        if (accounts.Length == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");
        for (var i = 0; i < accounts.Length; i++)
            Console.WriteLine($"    {i + 1}. {accounts[i].AccountNumber} | {Utils.FormatRupiah(accounts[i].Balance)}");

        Console.Write("Nomor pilihan: ");
        if (int.TryParse(Console.ReadLine(), out var index) && index >= 1 && index <= accounts.Length)
            return accounts[index - 1];

        Console.WriteLine("Pilihan rekening tidak valid.");
        return null;
    }

    private static int? ReadAmount(string prompt)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var amount) && amount > 0) return amount;

        Console.WriteLine("Jumlah tidak valid. Harus berupa bilangan bulat lebih dari 0.");
        return null;
    }
}