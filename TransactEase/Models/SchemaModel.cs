namespace TransactEase.Models;

public class TableSchemaModel
{
    public required string Name { get; set; } //Not used yet. will be used for dynamicform
    public required TableColumnModel[] Columns { get; set; }
    public required List<string> SearchableFields { get; set; }
}

public class TableColumnModel
{
    public required string Key { get; set; }
    public required string Label { get; set; }
    public bool IsDate { get; set; }
    public bool IsBoolean { get; set; }
}

public class FormFieldModel
{
    public required string Key { get; set; }
    public required string Label { get; set; }
    public required string Type { get; set; }
    public bool Required { get; set; }
    public string? LookupResource { get; set; }
    public string? DisplayField { get; set; }
    public List<FormOptionModel>? Options { get; set; }
}

public class FormModel
{
    public required string FormName { get; set; }
    public required FormFieldModel[] Fields { get; set; }
}

public class FormOptionModel {
    public required string Label { get; set; }
    public required string Value { get; set; }
}

