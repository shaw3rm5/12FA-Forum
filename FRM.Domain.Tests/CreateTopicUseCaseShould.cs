using FluentAssertions;
using Forum.Domain;
using Forum.Domain.Dtos;
using Forum.Domain.Exceptions;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Infrastructure;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;

namespace FRM.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly IRepository<Topic> _TopicRepository;
    private readonly IRepository<Forum.Infrastructure.Forum> _ForumRepository;
    private readonly IRepository<User> _UserRepository;
    private readonly CreateTopicUseCase _sut;
    private readonly ISetup<IGuidFactory, Guid> _createIdSetup;
    private readonly ISetup<IMomentProvider, DateTimeOffset> _momentSetup;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
        var dbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);
        _TopicRepository = new Repository<Topic>(dbContext);
        _UserRepository = new Repository<User>(dbContext);
        _ForumRepository = new Repository<Forum.Infrastructure.Forum>(dbContext);
        
        
        var guidFactory = new Mock<IGuidFactory>();
        _createIdSetup = guidFactory.Setup(factory => factory.CreateGuid());
        var momentProvider = new Mock<IMomentProvider>();
        _momentSetup = momentProvider.Setup(provider => provider.Now);

        
        _sut = new CreateTopicUseCase(_TopicRepository, guidFactory.Object, momentProvider.Object, _ForumRepository);
    }
    
    [Fact]
    public async Task ThrowForumNotFoundException_WhenForumIsNotMatch()
    {

        await _ForumRepository.AddAsync(new Forum.Infrastructure.Forum()
        {
            Id = Guid.Parse("5EAFD77B-6C2C-4924-848C-12ACC8375685"),
            Title = "Test Title",
        }, CancellationToken.None);
        
        var forumId = Guid.Parse("941E210D-16E5-40E4-B04B-F25D44B05147");
        var authorId = Guid.Parse("D026BFA7-CF32-4C5E-974B-A793535F792B");

        await _sut.Invoking(s => s.Execute(forumId, "Hello World", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFindException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("71431797-5E5F-46A8-99E0-9DE1ECD63149");
        var userId = Guid.Parse("F5EEFE5C-53EE-4DFA-A8EA-9C0A3E9C4427");
        
        await _ForumRepository.AddAsync(new Forum.Infrastructure.Forum()
        {
            Id = forumId,
            Title = "Existing Forum",
        }, CancellationToken.None);
        
        await _UserRepository.AddAsync(new User()
        {
            Id = userId,
            UserName = "Aleks",
        }, CancellationToken.None);

        _createIdSetup.Returns(Guid.Parse("AC737DE0-BBFC-41D7-8AC4-495AFFD4E149"));
        _momentSetup.Returns(new DateTimeOffset(new DateTime(2025, 04, 10, 19, 8, 52), TimeSpan.FromHours(2)));
        
        var actual = await _sut.Execute(forumId, "Hello World", userId, CancellationToken.None);

        actual.Should().BeEquivalentTo(new Topic
        {
            ForumId = forumId,
            Title = "Hello World",
        }, opt => opt.Including(t => t.Title).Including(t => t.ForumId));

        actual.Should().BeEquivalentTo(new TopicDto()
        {   
            Id = Guid.Parse("AC737DE0-BBFC-41D7-8AC4-495AFFD4E149"),
            Title = "Hello World",
            ForumId = forumId,
            Author = "Aleks",
            CreatedAt = new  DateTimeOffset(new DateTime(2025, 04, 10, 19, 8, 52), TimeSpan.FromHours(2)),
        });
    }
}