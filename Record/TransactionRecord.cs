namespace PracticeBank.Class;

public record TransactionRecord(DateTime Time, string Type, string AccountNumber, string OwnerName, int Amount);