using Domain.Models;
using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Dtos;
using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly IIdentityProvider _identityProvider;
    private readonly ICreateTopicStorage _storage;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _identityProvider = identityProvider;
        _storage = storage;
    }
    
     
    
    public async Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);
        
        var (forumId, title) = command;
        
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