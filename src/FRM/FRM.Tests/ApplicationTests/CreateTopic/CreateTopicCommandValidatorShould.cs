using FluentAssertions;
using FluentValidation;
using Forum.Application.UseCases.CreateTopic;
using Moq;

namespace FRM.Tests.ApplicationTests.CreateTopic;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator _sut = new();
    private readonly Mock<IValidator<CreateTopicCommand>> _validator;

    
    [Fact]
    public void ReturnAccess_WhenCommandIsValid()
    {
        var actual = _sut.Validate(new CreateTopicCommand(Guid.Parse("F8A97E00-1890-4172-9FD5-9F3DD04A5A7D"), "Hello world"));

        actual.IsValid.Should().BeTrue();

    }

    public static IEnumerable<object[]> InvalidData() 
    {
        var validCommand = new  CreateTopicCommand(Guid.Parse("EF5AC85D-0CEF-4BA6-B72C-A6F07D3F91DA"), "hello world");
        
        yield return new object[] { validCommand with { Title  = string.Empty } };
        yield return new object[] { validCommand with { Title   = "    "}};
        yield return new object[] { validCommand with { Title = string.Join("a", Enumerable.Range(0, 101)) } };
        yield return new object[] { validCommand with { ForumId = Guid.Empty } };
        
    }
    
    [Theory]
    [MemberData(nameof(InvalidData))]
    public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand command)
    {
        var actual = _sut.Validate(command);
        actual.IsValid.Should().BeFalse();
    }
    
}