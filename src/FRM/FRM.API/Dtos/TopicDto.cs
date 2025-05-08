namespace FRM.API.Dtos;

public record TopicDto(Guid ForumId, Guid UserId, Guid TopicId, string Title);