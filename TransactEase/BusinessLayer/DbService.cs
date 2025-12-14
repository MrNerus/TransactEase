using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace TransactEase.BusinessLayer
{
    public class DbService
    {
        private string? _connectionString;
        public string GetConnectionString()
        {
            if (_connectionString != null)
            {
                return _connectionString;
            }
            string jsonText = File.ReadAllText("./env/DbConnection.json");

            Dictionary<string, string> json = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ?? throw new Exception("Json file not found");

            _connectionString = $@"Host={json["Host"]};Port={json["Port"]};Database={json["Database"]};Username={json["User"]};Password={json["Password"]}";
            return _connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(GetConnectionString());
        }
    }
}
