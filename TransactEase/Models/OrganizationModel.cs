using TransactEase.Enums;

namespace TransactEase.Models;

public class OrganizationModel
{
    public required string Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string SwiftCode { get; set; }
    public required string Country { get; set; }
    public required OrganizationTypeEnum Type { get; set; }
    public required string Contact { get; set; }
    public required string Email { get; set; }
    public string? ParentCode { get; set; }
}
