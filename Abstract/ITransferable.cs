namespace Bank.Abstract;

public interface ITransferable
{
    public Task<bool> Transfer(Account target, int amount, CancellationToken ct);
}