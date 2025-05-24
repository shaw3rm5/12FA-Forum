using FluentAssertions;
using Forum.Application.UseCases.GetTopics;

namespace FRM.Tests.ApplicaionTests.GetTopics;

public class GetTopicsCommandShould
{
    public GetTopicsCommandValidator _sut = new();    
    
    
    [Fact]
    public void TestCommandValidation()
    {
        var command = new GetTopicsCommand(Guid.Parse("9464deb4-640e-45af-a888-c062ca8e3b21"), 10, 10);
        
        _sut.Validate(command).IsValid.Should().BeTrue();
    }


    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var query = new GetTopicsCommand(Guid.Parse("9464deb4-640e-45af-a888-c062ca8e3b21"), 10, 10);
        yield return new object[] { query with { ForumId = Guid.Empty} };
        yield return new object[] { query with { Skip = -1} };
        yield return new object[] { query with { Take = -1} };
        yield return new object[] { query with {ForumId = Guid.Empty, Skip = -152, Take = -112}};
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void TestCommandWithInvalidData(GetTopicsCommand query)
    {
        _sut.Validate(query).IsValid.Should().BeFalse();
    }
}