
namespace Forum.Infrastructure;

public class Topic
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; } 
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public Guid AuthorId { get; set; }
    
    public Guid ForumId { get; set; }
    
    public string Title { get; set; }
    
    public Forum Forum { get; set; }
    public User Author { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    
}