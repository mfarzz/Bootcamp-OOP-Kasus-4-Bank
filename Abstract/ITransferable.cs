namespace PracticeBank.Abstract;

public interface ITransferable
{
    bool Transfer(Account target, int amount);
}