using PracticeBank.Abstract;

namespace PracticeBank.Class;

// T dibatasi hanya untuk Account atau turunannya
public class Repository<TAccount> where TAccount : Account
{
    private readonly List<TAccount> _items = new();                      // menjaga urutan
    private readonly Dictionary<string, TAccount> _byNumber = new();     // pencarian cepat lewat nomor rekening

    public int Count => _items.Count;
    public IReadOnlyList<TAccount> All => _items;

    public bool Add(TAccount item)
    {
        if (!_byNumber.TryAdd(item.AccountNumber, item)) return false;
        _items.Add(item);
        return true;
    }

    public TAccount? FindByNumber(string accountNumber)
    {
        return _byNumber.TryGetValue(accountNumber, out var item) ? item : null;
    }
}