namespace TransactEase.Models;

public class SessionModel
{
    public required string SessionId { get; set; }
    public required string ConnectionString { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
