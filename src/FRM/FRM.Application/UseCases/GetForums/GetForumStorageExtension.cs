using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.GetForums;

public static class GetForumStorageExtension
{
    public static async Task<bool> ForumExistsAsync(this IGetForumStorage storage, Guid forumId, CancellationToken cancellationToken)
    {
        var forums = await storage.GetForums(cancellationToken);
        return forums.Any(f => f.Id == forumId);
    }

    public static async Task ThrowIfForumNotFound(this IGetForumStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        if (!await ForumExistsAsync(storage, forumId, cancellationToken))
        {
            throw new ForumNotFindException(forumId);
        }
    }
}