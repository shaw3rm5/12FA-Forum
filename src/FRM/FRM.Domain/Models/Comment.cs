namespace Domain.Models;

public class Comment
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public string Text { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid TopicId { get; set; }
    
    public User Author { get; set; }
    
    public Topic Topic { get; set; }
}