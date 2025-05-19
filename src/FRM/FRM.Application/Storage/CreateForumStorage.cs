using Forum.Application.UseCases.CreateForum;
using Forum.Infrastructure.Repository;

namespace Forum.Application.Storage;

public class CreateForumStorage : ICreateForumStorage
{
    private readonly IRepository<Domain.Models.Forum> _repository;
    private readonly IGuidFactory _guidFactory;
    
    public CreateForumStorage(
        IRepository<Domain.Models.Forum> repository,
        IGuidFactory guidFactory)
    {
        _repository = repository;
        _guidFactory = guidFactory;
    }
    
    public async Task<Domain.Models.Forum> CreateForumAsync(string title, CancellationToken cancellationToken)
    {
        
        var forum = new Domain.Models.Forum()
        {
            Title = title,
            Id = _guidFactory.CreateGuid()
        };
        
        await _repository
            .AddAsync(forum, cancellationToken);
        
        return forum;
    }
}