using System.Text.Json;
using TransactEase.Models;

namespace TransactEase.BusinessLayer
{
    public class SchemaService
    {
        private static readonly Dictionary<string, TableSchemaModel> Tables = new()
        {
            ["transactions"] = new TableSchemaModel
            {
                Name = "transactions",
                Columns = 
                [
                    new TableColumnModel{ Key = "id", Label = "Transaction ID" },
                    new TableColumnModel{ Key = "userId", Label = "User" },
                    new TableColumnModel{ Key = "receiverId", Label = "Receiver" },
                    new TableColumnModel{ Key = "amount", Label = "Amount" },
                    new TableColumnModel{ Key = "createdAt", Label = "Date", IsDate = true },
                ],
                SearchableFields = ["id", "userId", "receiverId"]
            },
            ["audit-logs"] = new TableSchemaModel
            {
                Name = "audit-logs",
                Columns = 
                [
                    new TableColumnModel{ Key = "timestamp", Label = "Timestamp", IsDate = true },
                    new TableColumnModel{ Key = "user", Label = "User" },
                    new TableColumnModel{ Key = "action", Label = "Action" },
                    new TableColumnModel{ Key = "details", Label = "Details" },
                ],
                SearchableFields = ["user", "action", "details"]
            },
            ["cards"] = new TableSchemaModel
            {
                Name = "cards",
                Columns = 
                [
                    new TableColumnModel{ Key = "cardNumber", Label = "Card Number" },
                    new TableColumnModel{ Key = "cardType", Label = "Card Type" },
                    new TableColumnModel{ Key = "status", Label = "Status" },
                    new TableColumnModel{ Key = "organizationId", Label = "Organization ID" },
                    new TableColumnModel{ Key = "userId", Label = "User ID" },
                    new TableColumnModel{ Key = "issueDate", Label = "Issue Date", IsDate = true },
                    new TableColumnModel{ Key = "expiryDate", Label = "Expiry Date", IsDate = true },
                ],
                SearchableFields = ["cardNumber", "organizationId", "userId"]
            },
            ["cashback-schemes"] = new TableSchemaModel
            {
                Name = "cashback-schemes",
                Columns = 
                [
                    new TableColumnModel{ Key = "name", Label = "Name" },
                    new TableColumnModel{ Key = "description", Label = "Description" },
                    new TableColumnModel{ Key = "rate", Label = "Rate" },
                    new TableColumnModel{ Key = "isActive", Label = "Active", IsBoolean = true },
                ],
                SearchableFields = ["name", "description"]
            },
            ["organizations"] = new TableSchemaModel
            {
                Name = "organizations",
                Columns =
                [
                    new TableColumnModel{ Key = "id", Label = "ID" },
                    new TableColumnModel{ Key = "name", Label = "Name" },
                    new TableColumnModel{ Key = "parentId", Label = "Parent ID" },
                    new TableColumnModel{ Key = "createdAt", Label = "Created At", IsDate = true },
                ],
                SearchableFields = ["id", "name", "parentId"]
            },
            ["staff"] = new TableSchemaModel
            {
                Name = "staff",
                Columns = 
                [
                    new TableColumnModel{ Key = "id", Label = "ID" },
                    new TableColumnModel{ Key = "fullName", Label = "Full Name" },
                    new TableColumnModel{ Key = "email", Label = "Email" },
                    new TableColumnModel{ Key = "organizationId", Label = "Organization ID" },
                    new TableColumnModel{ Key = "role", Label = "Role" },
                    new TableColumnModel{ Key = "isActive", Label = "Active", IsBoolean = true },
                    new TableColumnModel{ Key = "createdAt", Label = "Created At", IsDate = true },
                ],
                SearchableFields = ["fullName", "email", "organizationId", "role"]
            },
            ["users"] = new TableSchemaModel
            {
                Name = "users",
                Columns = 
                [
                    new TableColumnModel{ Key = "id", Label = "ID" },
                    new TableColumnModel{ Key = "fullName", Label = "Full Name" },
                    new TableColumnModel{ Key = "email", Label = "Email" },
                    new TableColumnModel{ Key = "organizationId", Label = "Organization ID" },
                    new TableColumnModel{ Key = "isActive", Label = "Active", IsBoolean = true },
                    new TableColumnModel{ Key = "createdAt", Label = "Created At", IsDate = true },
                ],
                SearchableFields = ["fullName", "email", "organizationId"]
            }
        };

        private static readonly Dictionary<string, FormModel> Forms = new()
        {
            ["transaction"] = new FormModel
            {
                FormName = "transaction",
                Fields = [
                    new FormFieldModel { Key = "userId", Label = "Sender", Type = "lookup", Required = true, LookupResource = "users", DisplayField = "name" },
                    new FormFieldModel { Key = "receiverId", Label = "Receiver", Type = "lookup", Required = true, LookupResource = "users", DisplayField = "name" },
                    new FormFieldModel { Key = "organizationId", Label = "Organization", Type = "lookup", Required = true, LookupResource = "organizations", DisplayField = "name" },
                    new FormFieldModel { Key = "amount", Label = "Amount", Type = "number", Required = true },
                    new FormFieldModel { Key = "cashbackId", Label = "Cashback Scheme", Type = "lookup", LookupResource = "cashback-schemes", DisplayField = "name" },
                ]
            },
            ["card"] = new FormModel
            {
                FormName = "card",
                Fields = [
                    new FormFieldModel { Key = "cardNumber", Label = "Card Number", Type = "text", Required = true },
                    new FormFieldModel { Key = "cardType", Label = "Card Type", Type = "select", Options = [new FormOptionModel { Label = "Debit", Value = "debit" }, new FormOptionModel { Label = "Credit", Value = "credit" } ], Required = true },
                    new FormFieldModel { Key = "issueDate", Label = "Issue Date", Type = "date", Required = true },
                    new FormFieldModel { Key = "expiryDate", Label = "Expiry Date", Type = "date", Required = true },
                    new FormFieldModel { Key = "status", Label = "Status", Type = "select", Options = [new FormOptionModel { Label = "Active", Value = "active" }, new FormOptionModel { Label = "Blocked", Value = "blocked" } ], Required = true },
                    new FormFieldModel { Key = "userId", Label = "Assigned User", Type = "lookup", Required = true, LookupResource = "users", DisplayField = "name" },
                    new FormFieldModel { Key = "cvv", Label = "CVV", Type = "text", Required = true }
                ]
            },
            ["card-assign"] = new FormModel
            {
                FormName = "card-assign",
                Fields = [
                    new FormFieldModel { Key = "userId", Label = "Assign To User", Type = "lookup", Required = true, LookupResource = "users", DisplayField = "name" },
                    new FormFieldModel { Key = "issueDate", Label = "Issue Date", Type = "date", Required = true },
                    new FormFieldModel { Key = "expiryDate", Label = "Expiry Date", Type = "date", Required = true }
                ]
            },
            ["card-transfer"] = new FormModel
            {
                FormName = "card-transfer",
                Fields = [
                    new FormFieldModel { Key = "targetOrganizationId", Label = "Transfer To", Type = "lookup", Required = true, LookupResource = "organizations", DisplayField = "name" }
                ]
            },
            ["cashback-scheme"] = new FormModel
            {
                FormName = "cashback-scheme",
                Fields = [
                    new FormFieldModel { Key = "name", Label = "Name", Type = "text", Required = true },
                    new FormFieldModel { Key = "description", Label = "Description", Type = "textarea", Required = true },
                    new FormFieldModel { Key = "rate", Label = "Rate (0.0 - 1.0)", Type = "number", Required = true },
                    new FormFieldModel { Key = "isActive", Label = "Active", Type = "boolean" },
                ]
            },
            ["organization"] = new FormModel
            {
                FormName = "organization",
                Fields = [
                    new FormFieldModel { Key = "name", Label = "Name", Type = "text", Required = true },
                    new FormFieldModel { Key = "parentId", Label = "Parent Organization", Type = "lookup", LookupResource = "organizations", DisplayField = "name" },
                ]
            },
            ["report-filter"] = new FormModel
            {
                FormName = "report-filter",
                Fields = [
                    new FormFieldModel { Key = "reportType", Label = "Report Type", Type = "select", Options = [new FormOptionModel { Label = "Transaction Report", Value = "report_1" }, new FormOptionModel { Label = "Cashback Report", Value = "report_2" }, new FormOptionModel { Label = "User Activity Report", Value = "report_3" } ], Required = true },
                    new FormFieldModel { Key = "startDate", Label = "Start Date", Type = "date", Required = true },
                    new FormFieldModel { Key = "endDate", Label = "End Date", Type = "date", Required = true }
                ]
            },
            ["staff"] = new FormModel
            {
                FormName = "staff",
                Fields = [
                    new FormFieldModel { Key = "fullName", Label = "Full Name", Type = "text", Required = true },
                    new FormFieldModel { Key = "email", Label = "Email", Type = "text", Required = true },
                    new FormFieldModel { Key = "organizationId", Label = "Organization", Type = "lookup", Required = true, LookupResource = "organizations", DisplayField = "name" },
                    new FormFieldModel { Key = "role", Label = "Role", Type = "select", Options = [new FormOptionModel { Label = "Admin", Value = "admin" }, new FormOptionModel { Label = "Manager", Value = "manager" }, new FormOptionModel { Label = "Staff", Value = "staff" } ], Required = true },
                    new FormFieldModel { Key = "isActive", Label = "Active", Type = "boolean" }
                ]
            },
            ["user"] = new FormModel
            {
                FormName = "user",
                Fields = [
                    new FormFieldModel { Key = "username", Label = "Username", Type = "text", Required = true },
                    new FormFieldModel { Key = "password", Label = "Password", Type = "password", Required = true },
                    new FormFieldModel { Key = "email", Label = "Email", Type = "text", Required = true },
                    new FormFieldModel { Key = "organizationId", Label = "Organization", Type = "lookup", Required = true, LookupResource = "organizations", DisplayField = "name" },
                    new FormFieldModel { Key = "role", Label = "Role", Type = "select", Options = [new FormOptionModel { Label = "Admin", Value = "admin" }, new FormOptionModel { Label = "Manager", Value = "manager" }, new FormOptionModel { Label = "Staff", Value = "staff" } ], Required = true },
                    new FormFieldModel { Key = "isActive", Label = "Active", Type = "boolean" }
                ]
            }
        };

        public TableSchemaModel? GetTableSchema(string tableName)
        {
            if (Tables.TryGetValue(tableName, out var schema))
            {
                return schema;
            }
            return null;
        }

        public FormModel? GetFormSchema(string formName)
        {
            if (Forms.TryGetValue(formName, out var schema))
            {
                return schema;
            }
            return null;
        }
    }
}
