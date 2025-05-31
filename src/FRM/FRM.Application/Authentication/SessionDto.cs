namespace Forum.Application.Authentication;

public class SessionDto
{
    public Guid UserId { get; set; }
    
    public DateTimeOffset ExpiredAt { get; set; }
    
}