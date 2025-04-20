namespace Forum.Application.Exceptions;

public class ForumNotFindException : ApplicationLayerException
{
    public ForumNotFindException(Guid forumId) 
        : base(ErrorCodes.Gone, $"Forum {forumId} does not exist") { }   
}

public abstract class ApplicationLayerException : Exception
{
    public ErrorCodes ErrorCode { get; }

    public ApplicationLayerException(ErrorCodes errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}