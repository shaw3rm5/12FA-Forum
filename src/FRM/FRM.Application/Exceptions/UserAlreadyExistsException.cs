namespace Forum.Application.Exceptions;

public class UserAlreadyExistsException : ApplicationLayerException
{
    public UserAlreadyExistsException(string login) 
        : base(ErrorCodes.Conflict, $"User {login} already exists.") { }
}