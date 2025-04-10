namespace Forum.Domain;

public interface IMomentProvider
{
    public DateTimeOffset Now { get; }
}

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
