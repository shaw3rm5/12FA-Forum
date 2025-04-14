
using Forum.Application.Dtos;

namespace Forum.Application.UseCases.GetForums;

public interface IGetForumStorage
{
    public Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken);
}