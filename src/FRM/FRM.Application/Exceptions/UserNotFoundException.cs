namespace Forum.Application.Exceptions;

public class UserNotFoundException : ApplicationLayerException
{
    public UserNotFoundException(Guid userId)
        : base(ErrorCodes.NotFound, $"User {userId} not found") { }
    public UserNotFoundException(string userName)
        : base(ErrorCodes.NotFound, $"User {userName} not found") { }
}