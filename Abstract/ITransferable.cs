namespace PracticeBank.Abstract;

public interface ITransferable
{
    public bool Transfer(Account target, int amount);
}