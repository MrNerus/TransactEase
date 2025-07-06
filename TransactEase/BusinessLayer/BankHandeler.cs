using Dapper;
using Npgsql;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Helper;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.BusinessLayer;

public class BankHandeler(BankDAL bankDal, DbConnectionModel dbConnection)
{
    private BankDAL _bankDal = bankDal;
        private readonly DbConnectionModel _dbConnection = dbConnection;
    
    public async Task<UserResponse> CreateBankAsync(BankModel bank)
    {
        try
        {
            _ = bank.ValidateBankModel();

            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            if (!string.IsNullOrEmpty(bank.ParentCode))
            {
                string checkBankHierarchyQuery = Query.CheckBankHierarchy;
                int? type = await connection.QueryFirstAsync<int>(checkBankHierarchyQuery, new { ParentCode = bank.ParentCode });
                if (type is null) throw new UserException(new { message = "Parent bank not found", invoker = "CreateBank", data = bank }, true, StatusEnum.ERROR);
                if (type > (int)bank.Type) throw new UserException(new { message = "Bank Type Hierarchy is not correct", invoker = "CreateBank", data = bank }, true, StatusEnum.ERROR);
            }
            
            string swiftCodeDuplicateQuery = Query.GetBank;


            try
            {
                _ = await _bankDal.CreateBankAsync(bank, connection, transaction);
            }
            catch (Exception e)
            {
                _ = e;
                await transaction.RollbackAsync();
                throw;
            }

            await transaction.CommitAsync();
            return new UserResponse { Message = "Bank created successfully", Status = StatusEnum.SUCCESS, Data = bank };

        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR, Data = bank };
        }
    }
    
    public async Task<UserResponse> UpdateBankAsync(BankModel bank)
    {
        try {
            _ = bank.ValidateBankModel();
            _ = await _bankDal.CreateBankAsync(bank);
            return new UserResponse { Message = "Bank created successfully", Status = StatusEnum.SUCCESS, Data = bank };

        } catch (Exception e) {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR, Data = bank };
        }
    }
}
