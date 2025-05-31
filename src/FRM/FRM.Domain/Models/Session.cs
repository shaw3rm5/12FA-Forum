namespace Domain.Models;

public class Session
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public DateTimeOffset ExpiredAt { get; set; }
}