using Forum.Application.Authentication;

namespace Forum.Application.UseCases.SignUp;

public interface ISignUpUseCase
{
    public Task<IIdentity> Execute(SignUpCommand command, CancellationToken cancellationToken);
}