namespace Bank.Exceptions;

public class DepositLockException(string message) : Exception(message);