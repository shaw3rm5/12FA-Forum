namespace Forum.Domain;

public interface IGuidFactory
{
    public Guid CreateGuid();
}

public class GuidFactory : IGuidFactory
{
    public Guid CreateGuid() => Guid.NewGuid();
}