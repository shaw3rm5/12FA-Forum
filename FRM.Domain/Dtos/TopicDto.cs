namespace Forum.Domain.Dtos;

public record TopicDto
{
    public Guid Id { get; init; }
    public Guid ForumId { get; init; }
    public string Title { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Author { get; init; }
}