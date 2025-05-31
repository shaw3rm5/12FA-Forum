using Forum.Application.Authentication;

namespace Forum.Application.UseCases.SignIn;

public interface ISignInUseCase
{
    Task<(IIdentity identity, string authToken)> Execute(SignInCommand command, CancellationToken cancellationToken);
}