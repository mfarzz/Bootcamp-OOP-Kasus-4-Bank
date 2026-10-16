namespace PracticeBank.Exceptions;

public class DepositLockedException(int monthsToMaturity)
    : Exception($"Deposito belum jatuh tempo ({monthsToMaturity} bulan lagi).")
{
    
}