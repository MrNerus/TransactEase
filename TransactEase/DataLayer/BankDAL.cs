using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using TransactEase.Enums;
using TransactEase.Helper;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.DataLayer
{
    public class BankDAL(DbConnectionModel dbConnection)
    {
        private readonly DbConnectionModel _dbConnection = dbConnection;

        public async Task<bool> CreateBankAsync(BankModel bank, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {

                string insertQuery = Query.CreateBank;

                int reader = await connection.ExecuteAsync(insertQuery, bank, transaction);

                if (reader == 1)
                {
                    _ = new UserException(new { message = "Bank created successfully", invoker = "CreateBank", data = bank }, true, StatusEnum.SUCCESS);
                    return true;
                }
                throw new UserException(new { message = "Error creating bank", invoker = "CreateBank", data = bank }, true, StatusEnum.ERROR);

            }
            catch (UserException e)
            {
                _ = e;
                throw;
            }
            catch (Exception e)
            {
                _ = new UserException(new { message = "Error creating bank", invoker = "CreateBank", data = new { DataIn = bank, Exception = e } }, true, StatusEnum.ERROR);
                throw;
            }
        }
        
        public async Task<bool> UpdateBankAsync(BankModel bank)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();
                using var transaction = await connection.BeginTransactionAsync();

                try {
                    string insertQuery = Query.CreateBank;
                    
                    int reader = await connection.ExecuteAsync(insertQuery, bank);
                    
                    if (reader == 1)
                    {
                        _ = new UserException(new { message = "Bank created successfully", invoker = "CreateBank", data = bank }, true, StatusEnum.SUCCESS);
                        return true;
                    }
                    throw new UserException(new {message = "Error creating bank", invoker = "CreateBank", data = bank}, true, StatusEnum.ERROR);
                } catch (Exception e) {
                    _ = e;
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            catch (Exception e)
            {
                if (e is not UserException) {
                    _ = new UserException(new {message = "Error creating bank", invoker = "CreateBank", data = new {DataIn = bank, Exception = e}}, true, StatusEnum.ERROR);
                }
                throw;
            }
        }
    }
}