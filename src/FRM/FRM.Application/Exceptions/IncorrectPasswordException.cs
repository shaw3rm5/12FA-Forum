namespace Forum.Application.Exceptions;

public class IncorrectPasswordException : ApplicationLayerException
{
    public IncorrectPasswordException(string password) 
        : base(ErrorCodes.BadRequest,$"Password {password} is incorrect") { }
}