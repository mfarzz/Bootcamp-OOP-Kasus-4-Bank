using Bank.Class;

namespace Bank;

public class Utils
{
    public static string FormatRupiah(int amount)
    {
        return $"Rp. {amount:N}";
    }
}