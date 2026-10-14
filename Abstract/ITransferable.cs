namespace Bank.Abstract;

public interface ITransferable
{
    public bool TransferTo(Account destination, double amount);
}