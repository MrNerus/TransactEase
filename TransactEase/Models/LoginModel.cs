namespace TransactEase.Models;

public class LoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string OrganizationIdentifier { get; set; }
    public string? ConnectionString { get; set; }
}
