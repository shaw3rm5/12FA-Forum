using Domain.Models;
using FluentAssertions;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Dtos;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.CreateTopic;
using Moq;
using Moq.Language.Flow;

namespace FRM.Application.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase _sut;
    private readonly Mock<ICreateTopicStorage> storage;
    private readonly ISetup<ICreateTopicStorage, Task<bool>> forumExistSetup;
    private readonly ISetup<ICreateTopicStorage, Task<TopicDto>> createTopicSetup;
    private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetop;
    private readonly ISetup<IIntentionManager, bool> intentionManagerSetup;
    private readonly Mock<IIntentionManager> intentionManager;

    public CreateTopicUseCaseShould()
    {
        
        storage = new Mock<ICreateTopicStorage>();
        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        intentionManager = new Mock<IIntentionManager>();
        
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        getCurrentUserIdSetop = identity.Setup(s => s.UserId);
        intentionManagerSetup = intentionManager.Setup(s => s.IsAllowed(It.IsAny<TopicIntention>()));
        
        forumExistSetup = storage.Setup(s => s.ForumExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        createTopicSetup = storage.Setup(s =>
            s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        
        _sut = new CreateTopicUseCase(intentionManager.Object, identityProvider.Object, storage.Object);
    }
    
    [Fact]
    public async Task ThrowForumNotFoundException_WhenForumIsNotMatch()
    {
        
        var forumId = Guid.Parse("941E210D-16E5-40E4-B04B-F25D44B05147");

        intentionManagerSetup.Returns(true);
        forumExistSetup.ReturnsAsync(false);

        await _sut.Invoking(s => s.Execute(forumId, "Hello World", CancellationToken.None))
            .Should().ThrowAsync<ForumNotFindException>();
        
        storage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
    }


    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreateIsNotAllowed()
    {
        var forumId = Guid.Parse("2E1D6F09-65FD-473C-A71A-D67BB6C3BC06");

        intentionManagerSetup.Returns(false);

        await _sut.Invoking(s => s.Execute(forumId, "Hello World", CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        intentionManager.Verify(s => s.IsAllowed(TopicIntention.Create));

    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMathingForumExists()
    {   
        var forumId = Guid.Parse("71431797-5E5F-46A8-99E0-9DE1ECD63149");
        var authorId = Guid.Parse("F5EEFE5C-53EE-4DFA-A8EA-9C0A3E9C4427");
        
        intentionManagerSetup.Returns(true);
        forumExistSetup.ReturnsAsync(true);
        getCurrentUserIdSetop.Returns(authorId);
        var excepted = new TopicDto();
        createTopicSetup.ReturnsAsync(excepted);
        

        
        var actual = await _sut.Execute(forumId, "Hello World", CancellationToken.None);
        actual.Should().Be(excepted);
        storage.Verify(s => s.CreateTopic(forumId, authorId, "Hello World", It.IsAny<CancellationToken>()), Times.Once);
        
    }
}