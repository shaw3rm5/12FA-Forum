using Forum.Domain.Dtos;
using Forum.Domain.Exceptions;
using Forum.Infrastructure;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IRepository<Topic> _repository;
    private readonly IRepository<Infrastructure.Forum> _forumRepository;
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;

    public CreateTopicUseCase(
        IRepository<Topic> repository,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider,
        IRepository<Infrastructure.Forum> forumRepository)
    {
        _repository = repository;
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
        _forumRepository = forumRepository;
    }
    
    public async Task<TopicDto> Execute(Guid forumId, string title, Guid userId, CancellationToken ct)
    {
        var forumExist = await _forumRepository.AnyAsync(f => f.Id == forumId, ct);
        if (!forumExist)
        {
            throw new ForumNotFindException(forumId);
        }
        
        var topicId = _guidFactory.CreateGuid();
        
        await _repository.AddAsync(new Topic()
        {
            Id = topicId,
            ForumId = forumId,
            Title = title,
            AuthorId = userId,
            CreatedAt = _momentProvider.Now,
        }, ct);


        return await _repository
            .Where(t => t.Id == topicId)
            .Select(t => new TopicDto
            {
                Id = t.Id,
                ForumId = t.ForumId,
                Title = t.Title,
                Author = t.Author.UserName,
                CreatedAt = t.CreatedAt
            })
            .FirstAsync(ct);
        

    }
}