using Forum.Application.Dtos;

namespace Forum.Application.UseCases.GetForums;

public interface IGetForumsUseCase
{
    Task<IEnumerable<ForumDto>> Execute(CancellationToken ct);
}