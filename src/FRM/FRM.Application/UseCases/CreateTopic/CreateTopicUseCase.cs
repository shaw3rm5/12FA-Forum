using Domain.Models;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Dtos;
using Forum.Application.Exceptions;

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
    
     
    
    public async Task<TopicDto> Execute(Guid forumId, string title, CancellationToken ct)
    {
        _identityProvider.Current.UserId = Guid.Parse("f5eefe5c-53ee-4dfa-a8ea-9c0a3e9c4427");
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        var forumExist = await _storage.ForumExists(forumId, ct);
        if (!forumExist)
        {
            throw new ForumNotFindException(forumId);
        }
        
        return await _storage.CreateTopic(forumId,_identityProvider.Current.UserId , title, ct);
        
    }
}