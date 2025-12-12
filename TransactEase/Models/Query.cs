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
        public static string CreateOrganization = "INSERT INTO ORGANIZATION (Name, Address, Code, SwiftCode, Country, Type, Contact, Email, ParentCode) VALUES (@Name, @Address, @Code, @SwiftCode, @Country, @Type, @Contact, @Email, @ParentCode);";
        public static string GetAllOrganizations = "SELECT * FROM ORGANIZATION WHERE IsActive = true;";
        public static string GetOrganization = "SELECT * FROM ORGANIZATION WHERE Id = @Id AND IsActive = true;";
        public static string UpdateOrganization = "UPDATE ORGANIZATION SET Name = @Name, Address = @Address, SwiftCode = @SwiftCode, Country = @Country, Type = @Type, Contact = @Contact, Email = @Email WHERE Id = @Id;";
        public static string DeleteOrganization = "UPDATE ORGANIZATION SET IsActive = false WHERE Id = @Id;";
        public static string CheckOrganizationHierarchy = "SELECT Type FROM ORGANIZATION WHERE ParentCode = @ParentCode;";

        public static string CreateOrganizationTable = @"
            CREATE TABLE IF NOT EXISTS ORGANIZATION (
                Id SERIAL PRIMARY KEY,
                Code VARCHAR(255) NOT NULL,
                Name VARCHAR(255) NOT NULL,
                Address VARCHAR(255) NOT NULL,
                SwiftCode VARCHAR(255) NOT NULL,
                Country VARCHAR(255) NOT NULL,
                Type INT NOT NULL,
                Contact VARCHAR(255) NOT NULL,
                Email VARCHAR(255) NOT NULL,
                ParentCode VARCHAR(255),
                IsActive BOOLEAN NOT NULL DEFAULT true
            );
        ";

        public static string CreateDbVersionTable = @"
            CREATE TABLE IF NOT EXISTS DB_VERSION (
                Version INT NOT NULL
            );
        ";

        public static string CreateUsersTable = @"
            CREATE TABLE IF NOT EXISTS USERS (
                Id SERIAL PRIMARY KEY,
                Username VARCHAR(255) NOT NULL UNIQUE,
                Password VARCHAR(255) NOT NULL,
                OrganizationId INT,
                IsActive BOOLEAN NOT NULL DEFAULT true,
                FOREIGN KEY (OrganizationId) REFERENCES ORGANIZATION(Id)
            );
        ";

        public static string CreateRolesTable = @"
            CREATE TABLE IF NOT EXISTS ROLES (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(255) NOT NULL UNIQUE
            );
        ";

        public static string CreateUserRolesTable = @"
            CREATE TABLE IF NOT EXISTS USER_ROLES (
                UserId INT NOT NULL,
                RoleId INT NOT NULL,
                PRIMARY KEY (UserId, RoleId),
                FOREIGN KEY (UserId) REFERENCES USERS(Id),
                FOREIGN KEY (RoleId) REFERENCES ROLES(Id)
            );
        ";

    }
}