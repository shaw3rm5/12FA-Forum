namespace Forum.Application.UseCases.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string? Title);