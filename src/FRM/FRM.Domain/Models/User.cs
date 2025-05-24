namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    
    public string Salt { get; set; }
    
    public string PasswordHash { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Topic> Topics { get; set; }
}