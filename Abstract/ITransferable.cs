namespace PracticeBank.Abstract;

public interface ITransferable
{
    bool Transfer(Account target, int amount);

    Task<bool> TransferInterbankAsync(string bankName, string destinationAccountNumber, int ammount,
        CancellationToken cancellationToken);
}