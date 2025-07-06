using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.Helper;

public static class OrganizationHelper
{
    public static bool ValidateOrganizationModel(this OrganizationModel organization)
    {
        List<string> emptyFields = new List<string>();
        if (string.IsNullOrEmpty(organization.Name)) emptyFields.Add("Name");
        if (string.IsNullOrEmpty(organization.Address)) emptyFields.Add("Address");
        if (string.IsNullOrEmpty(organization.SwiftCode)) emptyFields.Add("SwiftCode");
        if (string.IsNullOrEmpty(organization.Country)) emptyFields.Add("Country");
        if (organization.Type is 0) emptyFields.Add("Type");
        if (string.IsNullOrEmpty(organization.Contact)) emptyFields.Add("Contact");
        if (string.IsNullOrEmpty(organization.Email)) emptyFields.Add("Email");

        if (organization.Type == OrganizationTypeEnum.Branch || organization.Type == OrganizationTypeEnum.Counter)
        {
            if (string.IsNullOrEmpty(organization.ParentCode)) emptyFields.Add("ParentCode");
        }

        if (emptyFields.Count > 0)
        {
            throw new UserException(new { message = $"Following fields are empty: {string.Join(", ", emptyFields)}", invoker = "ClientIdPlaceholder", data = organization }, true, StatusEnum.ERROR);
        }
        return true;
    }
    public static bool ValidateExistingOrganizationModel(this OrganizationModel organization)
    {
        List<string> emptyFields = new List<string>();
        if (string.IsNullOrEmpty(organization.Id)) emptyFields.Add("Id");
        if (string.IsNullOrEmpty(organization.Name)) emptyFields.Add("Name");
        if (string.IsNullOrEmpty(organization.Address)) emptyFields.Add("Address");
        if (string.IsNullOrEmpty(organization.SwiftCode)) emptyFields.Add("SwiftCode");
        if (string.IsNullOrEmpty(organization.Country)) emptyFields.Add("Country");
        if (organization.Type is 0) emptyFields.Add("Type");
        if (string.IsNullOrEmpty(organization.Contact)) emptyFields.Add("Contact");
        if (string.IsNullOrEmpty(organization.Email)) emptyFields.Add("Email");

        if (organization.Type == OrganizationTypeEnum.Branch || organization.Type == OrganizationTypeEnum.Counter)
        {
            if (string.IsNullOrEmpty(organization.ParentCode)) emptyFields.Add("ParentCode");
        }

        if (emptyFields.Count > 0)
        {
            throw new UserException(new { message = $"Following fields are empty: {string.Join(", ", emptyFields)}", invoker = "ClientIdPlaceholder", data = organization }, true, StatusEnum.ERROR);
        }
        return true;
    }
}
