using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.Helper;

public static class BankHelper
{
    public static bool ValidateBankModel(this BankModel bank)
    {
        List<string> emptyFields = new List<string>();
        if (string.IsNullOrEmpty(bank.Name)) emptyFields.Add("Name");
        if (string.IsNullOrEmpty(bank.Address)) emptyFields.Add("Address");
        if (string.IsNullOrEmpty(bank.SwiftCode)) emptyFields.Add("SwiftCode");
        if (string.IsNullOrEmpty(bank.Country)) emptyFields.Add("Country");
        if (string.IsNullOrEmpty(bank.Contact)) emptyFields.Add("Contact");
        if (string.IsNullOrEmpty(bank.Email)) emptyFields.Add("Email");

        if (bank.Type == BankTypeEnum.Branch || bank.Type == BankTypeEnum.Outlet)
        {
            if (string.IsNullOrEmpty(bank.ParentCode)) emptyFields.Add("ParentCode");
        }

        if (emptyFields.Count > 0)
        {
            throw new UserException(new { message = $"Following fields are empty: {string.Join(", ", emptyFields)}", invoker = "ClientIdPlaceholder", data = bank }, true, StatusEnum.ERROR);
        }
        return true;
    }
    public static bool ValidateExistingBankModel(this BankModel bank)
    {
        List<string> emptyFields = new List<string>();
        if (string.IsNullOrEmpty(bank.Id)) emptyFields.Add("Id");
        if (string.IsNullOrEmpty(bank.Name)) emptyFields.Add("Name");
        if (string.IsNullOrEmpty(bank.Address)) emptyFields.Add("Address");
        if (string.IsNullOrEmpty(bank.SwiftCode)) emptyFields.Add("SwiftCode");
        if (string.IsNullOrEmpty(bank.Country)) emptyFields.Add("Country");
        if (string.IsNullOrEmpty(bank.Contact)) emptyFields.Add("Contact");
        if (string.IsNullOrEmpty(bank.Email)) emptyFields.Add("Email");

        if (bank.Type == BankTypeEnum.Branch || bank.Type == BankTypeEnum.Outlet)
        {
            if (string.IsNullOrEmpty(bank.ParentCode)) emptyFields.Add("ParentCode");
        }

        if (emptyFields.Count > 0)
        {
            throw new UserException(new { message = $"Following fields are empty: {string.Join(", ", emptyFields)}", invoker = "ClientIdPlaceholder", data = bank }, true, StatusEnum.ERROR);
        }
        return true;
    }
}
