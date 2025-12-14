using TransactEase.Enums;

namespace TransactEase.Models;

public class FieldMapping
{
    public required string SqlColumn { get; set; }
    public required FieldTypeEnum Type { get; set; }
}