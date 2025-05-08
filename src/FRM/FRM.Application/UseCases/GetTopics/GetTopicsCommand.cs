namespace Forum.Application.UseCases.GetTopics;

public record GetTopicsCommand(Guid ForumId, int Skip, int Take);