using System.Net.Http.Json;
using Domain.Models;
using FluentAssertions;
using Forum.Application.Authentication;
using Microsoft.Extensions.Caching.Hybrid;
using Xunit.Abstractions;

namespace FRM.Tests.IntegrationTests;

public class AccountEndpointShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory _factory;
    private readonly ITestOutputHelper _helper;

    private string login = "user";
    private string password = "password";
    
    public AccountEndpointShould(ForumApiApplicationFactory factory, ITestOutputHelper helper)
    {
        _factory = factory;
        _helper = helper;
    }

    [Fact]
    public async Task CheckIfRegisterIsSuccessful()
    {
        using var httpClient = _factory.CreateClient();

        using var SignUpRespone = await httpClient.PostAsync("/account", JsonContent.Create(new
        {
            login,
            password
        }));

        SignUpRespone.IsSuccessStatusCode.Should().BeTrue();
        var createdUser = await SignUpRespone.Content.ReadFromJsonAsync<UserIdentity>();
        
        using var SignInResponse = await httpClient.PostAsync("/account/sign-in", JsonContent.Create(new
        {
            login,
            password
        }));
        SignInResponse.Should().NotBeNull();
        SignInResponse.IsSuccessStatusCode.Should().BeTrue();

        var signedInUser = await SignInResponse.Content.ReadFromJsonAsync<UserIdentity>();
        signedInUser.UserId.Should().Be(createdUser.UserId);
        signedInUser.Should().NotBeNull();
        var createdForumResponse = await httpClient.PostAsync("/forum", JsonContent.Create(new
        {
            title = "nigger"
        }));
        createdForumResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}