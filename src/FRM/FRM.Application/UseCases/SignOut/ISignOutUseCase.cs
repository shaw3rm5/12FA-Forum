namespace Forum.Application.UseCases.SignOut;

public interface ISignOutUseCase
{
    public Task Execute(SignOutCommand commnd, CancellationToken cancellationToken);
}