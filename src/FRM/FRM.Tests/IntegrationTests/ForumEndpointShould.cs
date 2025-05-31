using System.Net.Http.Json;
using FluentAssertions;
using FRM.API.Dtos;

namespace FRM.Tests.IntegrationTests
{
    public class ForumEndpointShould : IClassFixture<ForumApiApplicationFactory>
    {
        private readonly ForumApiApplicationFactory _factory;

        public ForumEndpointShould(ForumApiApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task CreateForum()
        {
        
            using var client = _factory.CreateClient();
            
            using var initialForumResponse = await client.GetAsync("/Forum");
       
            initialForumResponse.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            initialForumResponse.EnsureSuccessStatusCode();
       
            var initialResult = await initialForumResponse.Content.ReadFromJsonAsync<ForumDto[]>();

            initialResult!
                .Should().BeEmpty();
            
            using var response = await client.PostAsync("/forum", JsonContent.Create(new
            {
                Title = "some title"
            }));
            
            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            response.EnsureSuccessStatusCode();
            
            var forum = await response.Content.ReadFromJsonAsync<ForumDto>();
            
            forum!
                .Title.Should().Be("some title");
            
        }
        
    }
}
