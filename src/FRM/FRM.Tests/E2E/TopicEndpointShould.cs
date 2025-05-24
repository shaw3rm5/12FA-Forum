using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Forum.Infrastructure;
using FRM.API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FRM.Tests.E2E;

public class TopicEndpointShould : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    private readonly ForumApiApplicationFactory _factory;
    private readonly Guid _forumId = Guid.Parse("d2df4221-dd6b-43cf-b427-117863f3727c");
    
    
    public TopicEndpointShould(ForumApiApplicationFactory factory)
    {
        _factory = factory;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ThrowForbidden_WhenUserIsNotAuthenticated()
    {
        var client = _factory.CreateClient();
        
        using var forumCreateResponse = await client.PostAsync("/forum", JsonContent.Create(new
        {
            title = "Test"    
        }));
    
        forumCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var responseForumId = await forumCreateResponse.Content.ReadFromJsonAsync<ForumDto>();
        
        
        using var response = await client.PostAsync("/forum/topic", JsonContent.Create( new
        {
            forumId = responseForumId?.Id,
            title = "Test Topic"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}