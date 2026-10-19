namespace Bank.Exceptions;

public class InsufficientBalanceException : Exception
{
   public int Balance { get; private set; }
   public int Amount { get; private set; }
   public InsufficientBalanceException(string message, int balance, int amount) :
      base($"{message}. Balance : {balance}, WithdrawalAmount: {amount}")
   {
      Balance = balance;
      Amount = amount;
   } 
}