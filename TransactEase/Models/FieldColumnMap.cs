using Microsoft.VisualBasic.FileIO;
using TransactEase.Enums;

namespace TransactEase.Models;

public static class FieldColumnMap
{
    public static Dictionary<string, FieldMapping> UserField = new()
    {
        { "customerName", new FieldMapping { SqlColumn = "c.full_name", Type = FieldTypeEnum.String, } },
        { "email", new FieldMapping { SqlColumn = "c.email", Type = FieldTypeEnum.String, } },
        { "trustScore", new FieldMapping { SqlColumn = "c.trust_score", Type = FieldTypeEnum.Number, } },
        { "balance", new FieldMapping { SqlColumn = "c.current_balance", Type = FieldTypeEnum.Number, } },
        { "transactionAmount", new FieldMapping { SqlColumn = "t.transaction_amount", Type = FieldTypeEnum.Number, } },
        { "transactionDate", new FieldMapping { SqlColumn = "t.transaction_date", Type = FieldTypeEnum.Date, } },
        { "status", new FieldMapping { SqlColumn = "t.status", Type = FieldTypeEnum.String, } }
    };
}