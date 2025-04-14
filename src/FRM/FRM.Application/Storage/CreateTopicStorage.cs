using Domain.Models;
using Forum.Application.Dtos;
using Forum.Application.UseCases.CreateTopic;
using Forum.Infrastructure;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class CreateTopicStorage : ICreateTopicStorage
{
    private readonly IRepository<Topic> _topicRepository;
    private readonly IRepository<Domain.Models.Forum> _forumRepository;
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;

    public CreateTopicStorage(
        IRepository<Topic> topicRepository,
        IRepository<Domain.Models.Forum> forumRepository,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider)
    {
        _topicRepository = topicRepository;
        _forumRepository = forumRepository;
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
    }
    
    public async Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken)
    {
        return await _forumRepository
            .AsQueryable()
            .AnyAsync(f => f.Id == forumId, cancellationToken);
    }

    public async Task<TopicDto> CreateTopic(Guid forumId, Guid authorId, string title, CancellationToken cancellationToken)
    {
        var topicDd = Guid.NewGuid();
        var topic = new Topic()
        {
            Id = topicDd,
            ForumId = forumId,
            Title = title,
            AuthorId = authorId,
            CreatedAt = _momentProvider.Now
        };
        
        await _topicRepository.AddAsync(topic, cancellationToken);
        
        return await _topicRepository
            .AsQueryable()
            .Where(f => f.Id == topic.Id)
            .Select(t => new TopicDto()
            {
                Id = t.Id,
                Author = t.Author.UserName,
                ForumId = t.ForumId,
                CreatedAt = t.CreatedAt,
                Title = t.Title,
            })
            .FirstAsync(cancellationToken);
    }
}