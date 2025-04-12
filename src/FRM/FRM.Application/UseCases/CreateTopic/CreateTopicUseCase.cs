using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Exceptions;
using Forum.Infrastructure;

namespace Forum.Application.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IIntentionManager _intentionManager;
    private readonly IIdentityProvider _identityProvider;
    private readonly ICreateTopicStorage _storage;

    public CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        _intentionManager = intentionManager;
        _identityProvider = identityProvider;
        _storage = storage;
    }
    
    public async Task<Topic> Execute(Guid forumId, string title, CancellationToken ct)
    {
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        var forumExist = await _storage.ForumExists(forumId, ct);
        if (!forumExist)
        {
            throw new ForumNotFindException(forumId);
        }
        
        return await _storage.CreateTopic(forumId, _identityProvider.Current.UserId, title, ct);
        
    }
}