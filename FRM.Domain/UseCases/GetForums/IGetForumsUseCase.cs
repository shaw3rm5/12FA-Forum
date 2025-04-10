using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.GetForums;

public interface IGetForumsUseCase
{
    Task<IEnumerable<ForumDto>> Execute(CancellationToken ct);
}