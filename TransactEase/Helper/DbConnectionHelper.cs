using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.Helper
{
    public static class DbConnectionHelper
    {
        public static string GetConnectionString(this DbConnectionModel dbConnection)
        {
            // return $"User ID={dbConnection.User};Password={dbConnection.Password};Host={dbConnection.Host};Port={dbConnection.Port};Database={dbConnection.Database};Pooling=true;";
            // return $"postgres://{dbConnection.User}:{dbConnection.Password}@{dbConnection.Host}:{dbConnection.Port}/{dbConnection.Database}";
            return $"Host={dbConnection.Host};Port={dbConnection.Port};Database={dbConnection.Database};Username={dbConnection.User};Password={dbConnection.Password}";
        }

        
    }
}