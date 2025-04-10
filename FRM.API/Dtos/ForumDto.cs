namespace FRM.API.ResponseDtos;

public record ForumDto
{
    public Guid Id { get; set; } 
    public string Title { get; set; }
}