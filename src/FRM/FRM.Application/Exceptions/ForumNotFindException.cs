namespace Forum.Application.Exceptions;

public class ForumNotFindException : Exception
{
    public ForumNotFindException(Guid forumId) 
        : base($"Forum {forumId} does not exist") { }   
}