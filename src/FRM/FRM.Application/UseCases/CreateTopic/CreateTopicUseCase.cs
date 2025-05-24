using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Dtos;
using Forum.Application.UseCases.GetForums;

namespace Forum.Application.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly IIdentityProvider _identityProvider;
    private readonly ICreateTopicStorage _storage;
    private readonly IGetForumStorage _forumStorage;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage,
        IGetForumStorage forumStorage)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _identityProvider = identityProvider;
        _storage = storage;
        _forumStorage = forumStorage;
    }
    
    public async Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var (forumId, title) = command;
        
        _identityProvider.Current.UserId = Guid.Parse("f5eefe5c-53ee-4dfa-a8ea-9c0a3e9c4427");
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        await _forumStorage.ThrowIfForumNotFound(forumId, cancellationToken);
        
        return await _storage.CreateTopic(forumId,_identityProvider.Current.UserId , title, cancellationToken);
        
    }
}