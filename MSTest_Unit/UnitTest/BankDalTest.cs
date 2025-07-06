namespace MSTest_Unit.UnitTest;
using TransactEase.Models;
using TransactEase.DataLayer;

[TestClass]
public sealed class BankDALTest
{
    public async void CreateBankAsyncTest_HappyPath()
    {
        DbConnectionModel dbConnection = new();
        BankDAL bankDal = new(dbConnection);
        BankModel bank = new() {
            Name = "Bank of America",
            Address = "Charlotte, North Carolina",
            SwiftCode = "BOFAUS3N",
            Country = "United States",
            Type = "Bank",
            Contact = "1-800-432-1000",
            Email = "support@bankofamerica.com"
        };

        string result = string.Empty;
        try {
            if (await bankDal.CreateBankAsync(bank)) return;
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
}
