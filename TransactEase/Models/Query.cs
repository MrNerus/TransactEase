using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactEase.Models
{
    public static class Query
    {
        public static string CreateBank = "INSERT INTO BANK (Name, Address, SwiftCode, Country, Type, Contact, Email, ParentCode) VALUES (@Name, @Address, @SwiftCode, @Country, @Type, @Contact, @Email, @ParentCode);";
        public static string CreateBankAccount = "INSERT INTO BANK_ACCOUNT (BankId, AccountNumber, AccountName, Currency, AccountType, Balance) VALUES (@BankId, @AccountNumber, @AccountName, @Currency, @AccountType, @Balance);";
        public static string CreateBankTransaction = "INSERT INTO BANK_TRANSACTION (AccountNumber, TransactionType, Amount, TransactionDate, Description) VALUES (@AccountNumber, @TransactionType, @Amount, @TransactionDate, @Description);";
        public static string CreateBankTransactionHistory = "INSERT INTO BANK_TRANSACTION_HISTORY (BankTransactionId, TransactionStatus, TransactionDate, Description) VALUES (@BankTransactionId, @TransactionStatus, @TransactionDate, @Description);";
        public static string GetBank = "SELECT * FROM BANK WHERE Id = @Id;";
        public static string GetBankAccount = "SELECT * FROM BANK_ACCOUNT WHERE AccountNumber = @AccountNumber;";
        public static string GetBankTransaction = "SELECT * FROM BANK_TRANSACTION WHERE BankTransactionId = @BankTransactionId;";
        public static string GetBankTransactionHistory = "SELECT * FROM BANK_TRANSACTION_HISTORY WHERE LogId = @LogId;";
        public static string GetBankAccounts = "SELECT * FROM BANK_ACCOUNT WHERE BankId = @BankId;";
        public static string GetBankTransactions = "SELECT * FROM BANK_TRANSACTION WHERE BankAccountId = @BankAccountId;";
        public static string GetBankTransactionHistories = "SELECT * FROM BANK_TRANSACTION_HISTORY WHERE BankTransactionId = @BankTransactionId;";
        public static string UpdateBank = "UPDATE BANK SET Name = @Name, Address = @Address, SwiftCode = @SwiftCode, Country = @Country, Type = @Type, Contact = @Contact, Email = @Email WHERE Id = @Id;";
        public static string UpdateBankAccount = "UPDATE BANK_ACCOUNT SET BankId = @BankId, AccountNumber = @AccountNumber, AccountName = @AccountName, Currency = @Currency, AccountType = @AccountType, Balance = @Balance WHERE Id = @Id;";
        public static string UpdateBankTransaction = "UPDATE BANK_TRANSACTION SET BankAccountId = @BankAccountId, TransactionType = @TransactionType, Amount = @Amount, TransactionDate = @TransactionDate, Description = @Description WHERE Id = @Id;";
        public static string CheckBankHierarchy = "SELECT Type FROM BANK WHERE ParentCode = @ParentCode;";
        public static string SelectBank = "SELECT * FROM BANK WHERE Id = @Id;";
        public static string CreateOrganization = "INSERT INTO ORGANIZATION (Name, Address, SwiftCode, Country, Type, Contact, Email, ParentCode) VALUES (@Name, @Address, @SwiftCode, @Country, @Type, @Contact, @Email, @ParentCode);";
        public static string GetOrganization = "SELECT * FROM ORGANIZATION WHERE Id = @Id;";
        public static string UpdateOrganization = "UPDATE ORGANIZATION SET Name = @Name, Address = @Address, SwiftCode = @SwiftCode, Country = @Country, Type = @Type, Contact = @Contact, Email = @Email WHERE Id = @Id;";
        public static string CheckOrganizationHierarchy = "SELECT Type FROM ORGANIZATION WHERE ParentCode = @ParentCode;";
        public static string SelectOrganization = "SELECT * FROM ORGANIZATION WHERE Id = @Id;";

    }
}