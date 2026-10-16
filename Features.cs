using System.Text;
using PracticeBank.Abstract;
using PracticeBank.AccountType;
using PracticeBank.Class;

namespace PracticeBank.Feature;

public class Features
{
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

        // return bisa diganti string pesan error
        if (!account.Withdraw(amount.Value))
        {
            Console.WriteLine("Penarikan gagal.");
            return;
        }

        Console.WriteLine($"Penarikan berhasil. Saldo sekarang: {Utils.FormatRupiah(account.Balance)}");
    }

    //Menu 4
    public static void Transfer(Account[] senderAccounts, Bank bank)
    {
        Account? source;
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
                Console.WriteLine("Rekening tujuan tidak ditemukan");
                Console.WriteLine("Silahkan inputkan kembali \n");
                continue;
            }

            if (ReferenceEquals(found, source))
            {
                Console.WriteLine("Rekening tujuan tidak boleh sama dengan rekening asal");
                Console.WriteLine("Silahkan inputkan kembali \n");
                continue;
            }

            destination = found;
            break;
        }

        var ammount = ReadAmount("Jumlah transefer: Rp ");
        if (ammount == null) return;

        if (!transferable.Transfer(destination, ammount.Value))
        {
            Console.WriteLine("Transfer gagal. Saldo tidak mencukupi atau batas rekening terlampaui");
            return;
        }

        Console.WriteLine("===========BUKTI TRANSFER==========");
        Console.WriteLine($"Transfer Rp{ammount.Value:N} ke {owner.Name} ({destination.AccountNumber}) berhasil");
        Console.WriteLine($"Saldo rekening asal sekarang: {Utils.FormatRupiah(source.Balance)}");
    }

    // Menu 5
    public void PrintMutation(Customer customer)
    {
        var acc = PickAccount(customer);
        if (acc == null)
        {
            Console.WriteLine("Akun tidak ditemukan.");
            return;
        }

        acc.PrintStatement();
    }

    // Menu 6
    public void EndMonthProcess(Customer customer)
    {
        var accounts = customer.Accounts;
        foreach (var acc in accounts) acc?.ApplyEndMonth();
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
                acc = new Saving(accNumber, customer, initialBalance);
                break;
            case Abstract.AccountType.Checking:
                acc = new Checking(accNumber, customer, initialBalance);
                break;
            case Abstract.AccountType.TimeDeposit:
                acc = new TimeDeposit(accNumber, customer, initialBalance);
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

    private static Account? PickAccount(Account[] accounts)
    {
        if (accounts.Length == 0)
        {
            Console.WriteLine("Nasabah belum memiliki rekening.");
            return null;
        }

        Console.WriteLine("Pilih rekening:");
        for (var i = 0; i < accounts.Length; i++)
        {
            Console.WriteLine($"    {i + 1}. {accounts[i].AccountNumber} | {Utils.FormatRupiah(accounts[i].Balance)}");
        }

        Console.Write("Nomor rekening: ");
        if (int.TryParse(Console.ReadLine(), out var index)
            && index >= 1 && index <= accounts.Length)
            return accounts[index - 1];

        Console.WriteLine("Pilihan rekening tidak valid.");
        return null;
    }

    private static int? ReadAmount(string input)
    {
        Console.Write(input);
        if (int.TryParse(Console.ReadLine(), out var amount) && amount > 0) return amount;

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
        var random = new Random();
        var result = new StringBuilder(length);

        for (var i = 0; i < length; i++)
        {
            // Generate random 1-9 digit
            var digit = random.Next(0, 10);
            result.Append(digit);
        }

        return result.ToString();
    }

    private double GetInputBalance()
    {
        while (true)
        {
            Console.Write("Saldo awal: ");
            var input = Console.ReadLine();
            try
            {
                return Convert.ToDouble(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Input invalid.");
            }
        }
    }
}