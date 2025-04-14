namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    
    
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Topic> Topics { get; set; }
}