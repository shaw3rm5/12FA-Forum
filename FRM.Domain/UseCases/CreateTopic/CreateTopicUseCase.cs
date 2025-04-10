using Forum.Domain.Dtos;
using Forum.Domain.Exceptions;
using Forum.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Forum.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly ForumDbContext _dbContext;
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;

    public CreateTopicUseCase(
        ForumDbContext dbContext,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider)
    {
        _dbContext = dbContext;
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
    }
    
    public async Task<TopicDto> Execute(Guid forumId, string title, Guid userId, CancellationToken ct)
    {
        var forumExist = await _dbContext.Forums.AnyAsync(f => f.Id == forumId, ct);
        if (!forumExist)
        {
            throw new ForumNotFindException(forumId);
        }
        
        var topicId = _guidFactory.CreateGuid();
        
        await  _dbContext.Topics.AddAsync( new Topic()
        {
            Id = topicId,
            ForumId = forumId,
            Title = title,
            AuthorId = userId,
        }, ct);
        await _dbContext.SaveChangesAsync(ct);


        return await _dbContext.Topics
            .AsNoTracking()
            .Where(t => t.Id == topicId)
            .Select(t => new TopicDto
            {
                Id = t.Id,
                ForumId = t.ForumId,
                Title = t.Title,
                Author = t.Author.UserName,
                CreatedAt = _momentProvider.Now,
            })
            .FirstAsync(ct);
        

    }
}